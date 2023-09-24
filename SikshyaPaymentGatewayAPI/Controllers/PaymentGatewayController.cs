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
            var model = CryptographyService.DecryptData<GetStudentBalanceModel>
                        (
                            q,
                            _encryptionKey,
                            _encryptionIV
                        );

            var balanceQuery = new GetStudentBalanceQuery(model);

            try
            {
                var balance = await _mediator.Send(balanceQuery);

                return Ok(balance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ReceiptEntry")]
        public async Task<IActionResult> MakeReceiptEntry([FromQuery] string q)
        {
            var receiptEntryModel = CryptographyService.DecryptData<ReceiptEntryModel>
                        (
                            q,
                            _encryptionKey,
                            _encryptionIV
                        );

            var receiptEntryCommand = new ReceiptEntryCommand(receiptEntryModel);

            try
            {
                var paymentReceipt = await _mediator.Send(receiptEntryCommand);

                if (!string.IsNullOrEmpty(paymentReceipt.TRANID))
                {
                    var notificationEntry = await _mediator.Send(new NotificationEntryCommand(paymentReceipt, receiptEntryModel));

                    return Ok(notificationEntry);
                }
                return BadRequest("Receipt could not be recorded!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /*
         * Esewa service integration
         * 
            [HttpGet("PaymentRequest")]
            public async Task<IActionResult> SendPaymentRequest([FromQuery] EsewaRequestModel esewaRequestModel)
            {
                return Ok( await _mediator.Send(new PaymentRequestQuery(esewaRequestModel)));
            }

            [HttpPost("PaymentVerification/{studentRegistrationNumber}")]
            public async Task<IActionResult> VerifyPaymentRequest([FromRoute] GetStudentBalanceModel studentBalanceModel, [FromBody] EsewaVerificationModel verificationModel)
            {
                return Ok(await _mediator.Send(new PaymentVerificationCommand(verificationModel, studentBalanceModel)));
            }
        */
    }
}
