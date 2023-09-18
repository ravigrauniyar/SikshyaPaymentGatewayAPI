using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NepaliDateConverter.Net;
using SikshyaPaymentGatewayAPI.Data;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Services;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly SikshyaDatabaseContext _databaseContext;
        private readonly IConnectionService _connectionService;
        public PaymentRepository(SikshyaDatabaseContext databaseContext, IConnectionService connectionService)
        {
            _databaseContext = databaseContext;
            _connectionService = connectionService;
        }

        public async Task<double> GetStudentBalanceFromDB(string clientId, string studentRegistrationNumber)
        {
            // Using partial table to access DRAMT and CRAMT from server's database

            var amountList = await _databaseContext.Set<TblTrnJournalPartial>()
                                    .Where(journal => journal.ACID == studentRegistrationNumber && journal.VOID == 0)
                                    .Select(journal => new { journal.DRAMT, journal.CRAMT })
                                    .ToListAsync();

            // Total debit amount
            var studentDrBalance = amountList.Sum(amtItem => amtItem.DRAMT);

            // Total credit amount
            var studentCrBalance = amountList.Sum(amtItem => amtItem.CRAMT);

            // Return student balance
            return studentDrBalance - studentCrBalance;
        }
        public Task<string> AddPaymentReceiptToDB(string clientId, string studentRegistrationNumber, float paymentAmount, string paymentFrom)
        {
            // Variable used to check if entry in both receipt and notification table occurred or not
            int rowsAffected = 0;

            // Convert English date to Nepali date
            var englishDate = DateTime.Now.Date;
            var dateConverter = DateConverter.ConvertToNepali(englishDate.Year, englishDate.Month, englishDate.Day);
            var nepaliDate = dateConverter.Year + "-" + dateConverter.Month + "-" + dateConverter.Day;

            // Entry for Receipt table
            OnlinePaymentReceipt paymentReceipt = new()
            {
                TRANID = Guid.NewGuid().ToString("N"),
                STREGNO = studentRegistrationNumber,
                PAYFROM = paymentFrom,
                PAYAMT = paymentAmount,
                TRANEDATE = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                TRANNDATE = nepaliDate,
                TRNTIME = TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"),
                TFLG = 2,
                RREMARKS = "",
                REFID = Guid.NewGuid().ToString("N")
            };

            // Database connection to insert receipt entry
            using (SqlConnection connection = new(_connectionService.GetConnectionString()))
            {
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
            }

            // Entry for Notification table
            OnlinePayNotification payNotification = new()
            {
                SCHOOLID = clientId,
                TRANID = paymentReceipt.TRANID,
                STREGNO = studentRegistrationNumber,
                PAYFROM = paymentFrom,
                PAYAMT = paymentAmount,
                EDATE = paymentReceipt.TRANEDATE,
                TRNTIME = paymentReceipt.TRNTIME,
                PAYFLG = paymentReceipt.TFLG,
                PAYREMARKS = ""
            };

            // Database Connection to insert notification entry
            using (SqlConnection connection = new(_connectionService.GetConnectionString()))
            {
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
            }
            // Return transaction id on successful entries else error message
            return Task.FromResult(rowsAffected == 2 ? paymentReceipt.TRANID : "Error occurred!");
        }
    }
}
