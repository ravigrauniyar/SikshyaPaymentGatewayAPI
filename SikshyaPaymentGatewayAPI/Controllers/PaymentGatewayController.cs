using MediatR;
using Microsoft.AspNetCore.Mvc;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Queries;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("/api/payment-gateway/")]
    public class PaymentGatewayController: Controller
    {
        private readonly IMediator _mediator;
        public PaymentGatewayController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("StudentBalance")]
        public async Task<IActionResult> GetStudentBalance([FromQuery] GetStudentBalanceModel model)
        {
            var balanceQuery = new GetStudentBalanceQuery(model);
            var balance = await _mediator.Send(balanceQuery);

            return Ok(balance);
        }

        [HttpPost("ReceiptEntry")]
        public async Task<IActionResult> MakeReceiptEntry(ReceiptEntryModel receiptEntryModel)
        {
            var receiptEntryCommand = new ReceiptEntryCommand(receiptEntryModel);

            var paymentReceipt = await _mediator.Send(receiptEntryCommand);

            if(!String.IsNullOrEmpty(paymentReceipt.TRANID))
            {
                var notificationEntry = await _mediator.Send(new NotificationEntryCommand(paymentReceipt, receiptEntryModel));
                
                if(!String.IsNullOrEmpty(notificationEntry.TRANID))
                {
                    return Ok(notificationEntry);
                }
            }
            return BadRequest("Receipt could not be recorded!");
        }

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
    }
}
