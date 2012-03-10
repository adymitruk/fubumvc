using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security.AntiForgery;
using FubuMVC.Core.UI.Extensibility;
using FubuMVC.Core.Urls;
using FubuMVC.HelloSpark.Controllers.Air;
using FubuMVC.Spark;

namespace FubuMVC.HelloSpark {
	public class HelloSparkRegistry : FubuRegistry {
		public HelloSparkRegistry() {
			IncludeDiagnostics(true);

			Applies
				.ToThisAssembly();

			Actions
				.IncludeClassesSuffixedWithController();

			ApplyHandlerConventions();
			//ApplyConvention<BehaviourConfigurationTest>();

			Routes
				.HomeIs<AdamInputModel>()
				.IgnoreControllerNamespaceEntirely()
				.IgnoreMethodSuffix("Command")
				.IgnoreMethodSuffix("Query")
				.ConstrainToHttpMethod(action => action.Method.Name.EndsWith("Command"), "POST")
				.ConstrainToHttpMethod(action => action.Method.Name.StartsWith("Query"), "GET");

			Policies.Add<AntiForgeryPolicy>();

			this.UseSpark();

			Views
				.TryToAttachWithDefaultConventions()
				.TryToAttachViewsInPackages()
				.RegisterActionLessViews(x => x.ViewModelType == typeof(AdamSuccessModel));

			HtmlConvention<SampleHtmlConventions>();

			Services(s => {
			         	s.FillType<IExceptionHandler, AsyncExceptionHandler>();
			         	s.ReplaceService<IUrlTemplatePattern, JQueryUrlTemplate>();
			         });

			this.Extensions()
				.For<AirViewModel>("extension-placeholder", x => "<p>Rendered from content extension.</p>");
		}
	}

	public class AdamTestController {
		public FubuContinuation DoSubmission(AdamInputModel adamInputModel) {
			if (adamInputModel.CreditCard == 0) return FubuContinuation.TransferTo(adamInputModel);
			if (adamInputModel.CreditCard % 2 == 0) return FubuContinuation.TransferTo(adamInputModel as AdamSuccessModel);
  		adamInputModel.Message = "Please enter an even number!";
		  return FubuContinuation.TransferTo(adamInputModel);
		}
	}

	public class AdamSuccessModel : AdamInputModel {}

	public class AdamInputModel {
		public int CreditCard { get; set; }
		public string Message { get; set; }
	}
}