using FubuMVC.Core.Continuations;

namespace FubuMVC.HelloSpark.Controllers.Adam
{
    public class AdamTestController {
        public FubuContinuation post_DoSubmission(AdamInputModel adamInputModel) {
            if (adamInputModel.CreditCard == 0) return FubuContinuation.TransferTo(new AdamResultModel{Message="No credit card number specified"});
            if (adamInputModel.CreditCard % 2 == 0) return FubuContinuation.TransferTo(new AdamSuccessModel { CreditCard = adamInputModel.CreditCard });
            var successModel = new AdamResultModel { Message = "Please enter an even number!", CreditCard = adamInputModel.CreditCard };
            return FubuContinuation.TransferTo(successModel);
        }

        public AdamInputModel Home(AdamResultModel input){
            return new AdamInputModel {Message = input.Message, CreditCard = input.CreditCard};
        }

			  public AdamSuccessView post_ShowResult(AdamSuccessModel adamSuccessModel) {
			  	return new AdamSuccessView {CreditCard = adamSuccessModel.CreditCard};
			  }
    }

	public class AdamSuccessView {
		public int CreditCard { get; set; }
	}

	public class AdamSuccessModel {
		public int CreditCard { get; set; }
	}
}