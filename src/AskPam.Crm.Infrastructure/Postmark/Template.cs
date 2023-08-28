using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Postmark
{
    public class EmailTemplate
    {
        public const string Html = @"
<!DOCTYPE html PUBLIC "" -//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title></title>
	<style media = ""all"" rel=""stylesheet"" type=""text/css"">/* Base ------------------------------ */
   
    body {
      width: 100% !important;
      height: 100%;
      margin: 0;
    }
    a {
      color: #3869D4;
    }

    /* Layout ------------------------------ */
    .email-wrapper {
      width: 100%;
      margin: 0;
      padding: 0;
    }
    .email-content {
      width: 100%;
      margin: 0;
      padding: 0;
    }

 

    /* Body ------------------------------ */
    .email-body {
      width: 100%;
      margin: 0;
      padding: 0;
    }

    .email-footer {
      width: 100%;
      margin: 0 auto;
      padding: 0;
      text-align: center;
	  background-color: #fefefe;
    }
    .email-footer p
{
    color: #AEAEAE;
    }
	</style>
</head>
<body>
<table cellpadding = ""0"" cellspacing=""0"" class=""email-wrapper"" width=""100%"">
	<tbody>
		<tr>
			<td align = ""center"" >

            <table cellpadding=""0"" cellspacing=""0"" class=""email-content"" width=""100%""><!-- Logo -->
				<tbody><!-- Email Body -->
					<tr>
						<td class=""email-body"" width=""100%"">
						<div>{{body}}</div>
						</td>
					</tr>
					<tr>
						<td><br />
						<br />
						&nbsp;</td>
					</tr>
					<tr>
						<td>
						<table align = ""center"" cellpadding=""0"" cellspacing=""0"" class=""email-footer"" width=""100%"">
							<tbody>
								<tr>
									<td class=""content-cell"">
									<div style = ""padding-top:10px"" ><a href=""http://ask-pam.com""><span class=""sg-image"" data-imagelibrary=""%7B%22width%22%3A%22150%22%2C%22height%22%3A%2245%22%2C%22alignment%22%3A%22center%22%2C%22src%22%3A%22https%3A//marketing-image-production.s3.amazonaws.com/uploads/a090fb71560617cf488646a0caafe8538fe8ba8fc789bf791b9ac1d336ad328a361fc9f037c24cc13248781870b1e33c191835e788333ddba050f9fb759f7532.png%22%2C%22alt_text%22%3A%22%22%2C%22link%22%3A%22%22%2C%22classes%22%3A%7B%22sg-image%22%3A1%7D%7D"" style=""float: none; display: block; text-align: center;""><img height = ""45"" src=""https://marketing-image-production.s3.amazonaws.com/uploads/a090fb71560617cf488646a0caafe8538fe8ba8fc789bf791b9ac1d336ad328a361fc9f037c24cc13248781870b1e33c191835e788333ddba050f9fb759f7532.png"" style=""width: 150px; height: 45px;"" width=""150"" /></span></a></div>
									</td>
								</tr>
							</tbody>
						</table>
						</td>
					</tr>
				</tbody>
			</table>
			</td>
		</tr>
	</tbody>
</table>

            ";
    }
}
