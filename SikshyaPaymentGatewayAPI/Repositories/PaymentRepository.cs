using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NepaliDateConverter.Net;
using SikshyaPaymentGatewayAPI.Data;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly DynamicDbContext _dbContextService;
        private IConnectionRepository _connectionRepository;
        public PaymentRepository(DynamicDbContext dynamicDbContextService, IConnectionRepository connectionRepository)
        {
            _dbContextService = dynamicDbContextService;
            _connectionRepository = connectionRepository;
        }
        public async Task<double> GetStudentBalanceFromDB(GetStudentBalanceModel showBalanceModel)
        {
            var dbConnectionModel = new DbConnectionModel
            {
                clientId = showBalanceModel.clientId,
                serverIp = showBalanceModel.serverIp,
                database = showBalanceModel.database,
                loginId = showBalanceModel.loginId,
                password = showBalanceModel.password
            };
            var databaseContext = await _connectionRepository.UpdateDbContext(dbConnectionModel);
            // Using partial table to access DRAMT and CRAMT from server's database

            var amountList = await databaseContext.Set<TblTrnJournalPartial>()
                                    .Where(journal => journal.ACID == showBalanceModel.stdRegNo && journal.VOID == 0)
                                    .Select(journal => new { journal.DRAMT, journal.CRAMT })
                                    .ToListAsync();

            // Total debit amount
            var studentDrBalance = amountList.Sum(amtItem => amtItem.DRAMT);

            // Total credit amount
            var studentCrBalance = amountList.Sum(amtItem => amtItem.CRAMT);

            // Return student balance
            return studentDrBalance - studentCrBalance;
        }
        public async Task<OnlinePaymentReceipt> AddPaymentReceiptToDB(ReceiptEntryModel model)
        {
            // Variable used to check if entry in receipt occurred or not
            int rowsAffected = 0;

            // Convert English date to Nepali date
            var englishDate = DateTime.Now.Date;
            var dateConverter = DateConverter.ConvertToNepali(englishDate.Year, englishDate.Month, englishDate.Day);
            var nepaliDate = dateConverter.Year + "-" + dateConverter.Month.ToString("00") + "-" + dateConverter.Day.ToString("00");

            // Entry for Receipt table
            OnlinePaymentReceipt paymentReceipt = new()
            {
                TRANID = Guid.NewGuid().ToString("N"),
                STREGNO = model.studentRegistrationNumber,
                PAYFROM = model.paymentFrom,
                PAYAMT = model.paymentAmount,
                TRANEDATE = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                TRANNDATE = nepaliDate,
                TRNTIME = TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"),
                TFLG = 2,
                RREMARKS = "",
                REFID = Guid.NewGuid().ToString("N")
            };

            var dbConnectionModel = new DbConnectionModel
            {
                clientId = model.clientId,
                serverIp = model.serverIp,
                database = model.database,
                loginId = model.loginId,
                password = model.password
            };
            await _connectionRepository.UpdateDbContext(dbConnectionModel);

            // Database connection to insert receipt entry
            using SqlConnection connection = new(_connectionRepository.GetDbConnectionString());

            connection.Open();

            string sql = "INSERT INTO ONLINEPAYMENT_Receipt(TRANID, STREGNO, PAYFROM, PAYAMT, TRANEDATE, TRANNDATE, TRNTIME, TFLG, RREMARKS, REFID) " +
                            "VALUES (@TranId, @StRegNo, @PayFrom, @PayAmt, @TranEDate, @TranNDate, @TrnTime, @TFlg, @RRemarks, @RefId)";

            using SqlCommand cmd = new(sql, connection);

            cmd.Parameters.AddWithValue("@TranId", paymentReceipt.TRANID);
            cmd.Parameters.AddWithValue("@StRegNo", paymentReceipt.STREGNO);
            cmd.Parameters.AddWithValue("@PayFrom", paymentReceipt.PAYFROM);
            cmd.Parameters.AddWithValue("@PayAmt", paymentReceipt.PAYAMT);
            cmd.Parameters.AddWithValue("@TranEDate", paymentReceipt.TRANEDATE);
            cmd.Parameters.AddWithValue("@TranNDate", paymentReceipt.TRANNDATE);
            cmd.Parameters.AddWithValue("@TrnTime", paymentReceipt.TRNTIME);
            cmd.Parameters.AddWithValue("@TFlg", paymentReceipt.TFLG);
            cmd.Parameters.AddWithValue("@RRemarks", paymentReceipt.RREMARKS);
            cmd.Parameters.AddWithValue("@RefId", paymentReceipt.REFID);

            rowsAffected += cmd.ExecuteNonQuery();

            return rowsAffected == 1 ? paymentReceipt : new OnlinePaymentReceipt();
        }
        public async Task<OnlinePayNotification> AddPaymentNotificationToDB(OnlinePaymentReceipt paymentReceipt, ReceiptEntryModel model)
        {
            // Variable used to check if entry notification table occurred or not
            int rowsAffected = 0;

            // Entry for Notification table
            OnlinePayNotification payNotification = new()
            {
                TRANID = paymentReceipt.TRANID,
                STREGNO = paymentReceipt.STREGNO,
                PAYFROM = paymentReceipt.PAYFROM,
                PAYAMT = paymentReceipt.PAYAMT,
                EDATE = paymentReceipt.TRANEDATE,
                TRNTIME = paymentReceipt.TRNTIME,
                PAYFLG = paymentReceipt.TFLG,
                PAYREMARKS = ""
            };

            var dbConnectionModel = new DbConnectionModel
            {
                clientId = model.clientId,
                serverIp = model.serverIp,
                database = model.database,
                loginId = model.loginId,
                password = model.password
            };
            await _connectionRepository.UpdateDbContext(dbConnectionModel);

            // Database connection to insert receipt entry
            using SqlConnection connection = new(_connectionRepository.GetDbConnectionString());
            
            connection.Open();

            string sql = "INSERT INTO ONLINEPAY_NOTIFICATION (SCHOOLID, TRANID, STREGNO, EDATE, TRNTIME, PAYAMT, PAYREMARKS, PAYFLG, PAYFROM) " +
                            "VALUES (@SchoolId, @TranId, @StRegNo, @Edate, @TrnTime, @PayAmt, @PayRemarks, @PayFlg, @PayFrom)";

            using SqlCommand cmd = new(sql, connection);

            cmd.Parameters.AddWithValue("@SchoolId", payNotification.SCHOOLID);
            cmd.Parameters.AddWithValue("@TranId", payNotification.TRANID);
            cmd.Parameters.AddWithValue("@StRegNo", payNotification.STREGNO);
            cmd.Parameters.AddWithValue("@Edate", payNotification.EDATE);
            cmd.Parameters.AddWithValue("@TrnTime", payNotification.TRNTIME);
            cmd.Parameters.AddWithValue("@PayAmt", payNotification.PAYAMT);
            cmd.Parameters.AddWithValue("@PayRemarks", payNotification.PAYREMARKS);
            cmd.Parameters.AddWithValue("@PayFlg", payNotification.PAYFLG);
            cmd.Parameters.AddWithValue("@PayFrom", payNotification.PAYFROM);

            rowsAffected += cmd.ExecuteNonQuery();

            return rowsAffected == 2 ? payNotification : new OnlinePayNotification();
        }
        public async Task<string> AddTransactionToJournal(double amount, GetStudentBalanceModel model)
        {
            int rowsAffected = 0;

            var dbConnectionModel = new DbConnectionModel
            {
                clientId = model.clientId,
                serverIp = model.serverIp,
                database = model.database,
                loginId = model.loginId,
                password = model.password
            };
            await _connectionRepository.UpdateDbContext(dbConnectionModel);

            // Database connection to insert receipt entry
            using SqlConnection connection = new(_connectionRepository.GetDbConnectionString());
            
            connection.Open();

            string sql = "INSERT INTO TBLTRNJOURNAL(VCHRNO, ACID, DRAMT, CRAMT, VOID) VALUES(@VCHRNO, @ACID, @DRAMT, @CRAMT, @VOID)";

            using SqlCommand cmd = new(sql, connection);

            cmd.Parameters.AddWithValue("@VCHRNO", 1234);
            cmd.Parameters.AddWithValue("@ACID", model.stdRegNo);
            cmd.Parameters.AddWithValue("@DRAMT", 0);
            cmd.Parameters.AddWithValue("@CRAMT", amount);
            cmd.Parameters.AddWithValue("@VOID", 0);

            rowsAffected += cmd.ExecuteNonQuery();

            if (rowsAffected == 1)
            {
                var studentBalance = GetStudentBalanceFromDB(model);
                return "Updated Student balance: " +  studentBalance.Result.ToString();
            }
            else return "Journal did not update!";
        }
    }
}
