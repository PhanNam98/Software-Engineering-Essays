#pragma checksum "C:\Users\Admin\Documents\GitHub\Software-Engineering-Essays\Source Code\Source code\.Net Core\BDS_ML\BDS_ML\Areas\Identity\Pages\Account\AccessDenied.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "548d9e2f0a5fc48c4e6abcc18f0d6c1208713368"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BDS_ML.Areas.Identity.Pages.Account.Areas_Identity_Pages_Account_AccessDenied), @"mvc.1.0.razor-page", @"/Areas/Identity/Pages/Account/AccessDenied.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Areas/Identity/Pages/Account/AccessDenied.cshtml", typeof(BDS_ML.Areas.Identity.Pages.Account.Areas_Identity_Pages_Account_AccessDenied), null)]
namespace BDS_ML.Areas.Identity.Pages.Account
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Admin\Documents\GitHub\Software-Engineering-Essays\Source Code\Source code\.Net Core\BDS_ML\BDS_ML\Areas\Identity\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\Users\Admin\Documents\GitHub\Software-Engineering-Essays\Source Code\Source code\.Net Core\BDS_ML\BDS_ML\Areas\Identity\Pages\_ViewImports.cshtml"
using BDS_ML.Areas.Identity;

#line default
#line hidden
#line 3 "C:\Users\Admin\Documents\GitHub\Software-Engineering-Essays\Source Code\Source code\.Net Core\BDS_ML\BDS_ML\Areas\Identity\Pages\_ViewImports.cshtml"
using BDS_ML.Models;

#line default
#line hidden
#line 1 "C:\Users\Admin\Documents\GitHub\Software-Engineering-Essays\Source Code\Source code\.Net Core\BDS_ML\BDS_ML\Areas\Identity\Pages\Account\_ViewImports.cshtml"
using BDS_ML.Areas.Identity.Pages.Account;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"548d9e2f0a5fc48c4e6abcc18f0d6c1208713368", @"/Areas/Identity/Pages/Account/AccessDenied.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a44bc481f36da85b94691e560ac4297fb37d14e3", @"/Areas/Identity/Pages/_ViewImports.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1f4dfd3e51ada8825bf096eec7d9639ae4a18ad8", @"/Areas/Identity/Pages/Account/_ViewImports.cshtml")]
    public class Areas_Identity_Pages_Account_AccessDenied : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\Admin\Documents\GitHub\Software-Engineering-Essays\Source Code\Source code\.Net Core\BDS_ML\BDS_ML\Areas\Identity\Pages\Account\AccessDenied.cshtml"
  
    ViewData["Title"] = "Truy cập bị từ chối";

#line default
#line hidden
            BeginContext(88, 40, true);
            WriteLiteral("\r\n<header>\r\n    <h1 class=\"text-danger\">");
            EndContext();
            BeginContext(129, 17, false);
#line 8 "C:\Users\Admin\Documents\GitHub\Software-Engineering-Essays\Source Code\Source code\.Net Core\BDS_ML\BDS_ML\Areas\Identity\Pages\Account\AccessDenied.cshtml"
                       Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(146, 98, true);
            WriteLiteral("</h1>\r\n    <p class=\"text-danger\">Bạn không có quyền truy cập vào tài nguyên này.</p>\r\n</header>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AccessDeniedModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<AccessDeniedModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<AccessDeniedModel>)PageContext?.ViewData;
        public AccessDeniedModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
