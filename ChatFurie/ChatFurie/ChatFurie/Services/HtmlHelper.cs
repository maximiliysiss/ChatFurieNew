using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatFurie.Services
{
    public static class HtmlHepler
    {
        public static string StringLengthValidate(this IHtmlHelper htmlHelper, string input, bool isConversation = false)
        {
            if (input.Length > (isConversation ? Constants.STRLEN_C : Constants.STRLEN))
                return $"{input.Substring(0, (isConversation ? Constants.STRLEN_C : Constants.STRLEN - 3))}...";
            return input;
        }

        public static string GetUserCondition(this IHtmlHelper htmlHelper, int id)
        {
            throw new NotImplementedException();
        }
    }
}
