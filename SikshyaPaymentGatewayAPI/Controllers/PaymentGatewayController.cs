using MediatR;
using Microsoft.AspNetCore.Mvc;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Queries;
using SikshyaPaymentGatewayAPI.Models;
using SikshyaPaymentGatewayAPI.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SikshyaPaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("/api/payment-gateway/")]
    public class PaymentGatewayController: Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly string _encryptionKey;
        private readonly string _encryptionIV;
        public PaymentGatewayController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;

            _encryptionKey = _configuration.GetValue<string>("encryption-key")!;
            _encryptionIV = _configuration.GetValue<string>("encryption-iv")!;
        }

        /*
         * Endpoints to get encrypted queries for StudentBalance and ReceiptEntry endpoints 
         * 
            [HttpPost("BalanceEncryption")]
            public IActionResult GetEncryptedBalanceQuery([FromBody] GetStudentBalanceModel jsonModel)
            {
                var encryptedJson = CryptographyService.EncryptData(jsonModel, _encryptionKey, _encryptionIV);
            
                return Ok(encryptedJson);
            }

            [HttpPost("ReceiptEncryption")]
            public IActionResult GetEncryptedReceiptQuery([FromBody] ReceiptEntryModel jsonModel)
            {
                var encryptedJson = CryptographyService.EncryptData(jsonModel, _encryptionKey, _encryptionIV);

                return Ok(encryptedJson);
            }
        */

        [HttpGet("StudentBalance")]
        public async Task<IActionResult> GetStudentBalance([FromQuery] string q)
        {
            try
            {
                var model = CryptographyService.DecryptData<GetStudentBalanceModel>(q, _encryptionKey, _encryptionIV);

                var balanceQuery = new GetStudentBalanceQuery(model);
                var balance = await _mediator.Send(balanceQuery);

                return balance.isSuccess ? Ok(balance) : BadRequest(balance);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseModel<string>.AsFailure(ex.Message));
            }
        }

        [HttpPost("ReceiptEntry")]
        public async Task<IActionResult> MakeReceiptEntry([FromQuery] string q)
        {
            try
            {
                var receiptEntryModel = CryptographyService.DecryptData<ReceiptEntryModel>(q, _encryptionKey, _encryptionIV);

                var receiptEntryCommand = new ReceiptEntryCommand(receiptEntryModel);
                var paymentReceipt = await _mediator.Send(receiptEntryCommand);

                if (paymentReceipt.isSuccess)
                {
                    var notificationEntry = await _mediator.Send(new NotificationEntryCommand(paymentReceipt.data!, receiptEntryModel));

                    return notificationEntry.isSuccess ? Ok(notificationEntry) : BadRequest(notificationEntry);
                }
                else return BadRequest(paymentReceipt);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseModel<string>.AsFailure(ex.Message));
            }
        }
    }
}
