using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBuenSabor.Tools
{
    public class HtmlTemplate
    {

        private string _html;

        public HtmlTemplate(string templatePath)
        {
            using (var reader = new StreamReader(templatePath, Encoding.UTF8))
                _html = reader.ReadToEnd();
        }

        public string Render(object values)
        {
            string output = _html;
            foreach (var p in values.GetType().GetProperties())
                output = output.Replace("[" + p.Name + "]", (p.GetValue(values, null) as string) ?? string.Empty);
            return output;
        }

    }
}
