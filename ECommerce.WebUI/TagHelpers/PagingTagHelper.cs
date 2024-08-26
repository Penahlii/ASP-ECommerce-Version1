using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace ECommerce.WebUI.TagHelpers
{
	[HtmlTargetElement("product-list-pager")]
	public class PagingTagHelper : TagHelper
	{
		[HtmlAttributeName("page-size")]
		public int PageSize { get; set; }
		[HtmlAttributeName("page-count")]
		public int PageCount { get; set; }
		[HtmlAttributeName("current-category")]
		public int CurrentCategory { get; set; }
		[HtmlAttributeName("current-page")]
		public int CurrentPage { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "section";

			var sb = new StringBuilder();
			if (PageCount > 1)
			{
				sb.Append("<ul class='pagination'>");

				var prevDisabled = (CurrentPage == 1) ? "disabled" : "";
				sb.AppendFormat("<li class='page-item {0} me-2'>", prevDisabled);
				sb.AppendFormat("<a class='page-link' href='/product/index?page={0}&category={1}' tabindex='-1'>Previous</a>",
					CurrentPage - 1, CurrentCategory);
				sb.Append("</li>");

				sb.AppendFormat("<li class='page-item active me-2'>");
				sb.AppendFormat("<a class='page-link'>{0}</a>", CurrentPage);
				sb.Append("</li>");

				var nextDisabled = (CurrentPage == PageCount) ? "disabled" : "";
				sb.AppendFormat("<li class='page-item {0} me-2'>", nextDisabled);
				sb.AppendFormat("<a class='page-link' href='/product/index?page={0}&category={1}'>Next</a>",
					CurrentPage + 1, CurrentCategory);
				sb.Append("</li>");

				sb.Append("</ul>");
			}

			output.Content.SetHtmlContent(sb.ToString());
		}
	}
}
