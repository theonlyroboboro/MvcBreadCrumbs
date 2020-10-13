using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MvcBreadCrumbs
{

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
	public class BreadCrumbAttribute : ActionFilterAttribute
	{
		public bool Clear { get; set; }

		public string Label { get; set; }

		public Type ResourceType { get; set; }

		/// <summary>
		/// If set to true, prevents the <see cref="BreadCrumbAttribute"/> at the controller level to process the action.
		/// Use when you want to add dynamic breadcrumb trails in code 
		/// but still allows other actions to use the attribute at the controller level.
		/// </summary>
		public bool Manual { get; set; }
				
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{			
			if (filterContext.HttpContext.Request.Method  != "GET")
				return;

			if (Clear)
			{
				StateManager.RemoveState(filterContext.HttpContext.Session.Id);
			}

			if (Manual)
				return;

			var state = StateManager.GetState(filterContext.HttpContext.Session.Id);
			state.Push(filterContext, Label, ResourceType);
		}
	}
}