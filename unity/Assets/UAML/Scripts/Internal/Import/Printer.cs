using System;
using System.Text;

namespace Uaml.Internal
{
    public class Printer
    {
        public class Scope : IDisposable
        {
            private Printer p;

            public Scope(Printer p)
            {
                this.p = p;
                p.BeginScope();
            }

            public void Dispose()
            {
                p.EndScope();
            }
        }

        private StringBuilder sb = new StringBuilder();

        private int i;
        public string Prefix { get; private set; }

        public void Indent(int n = 1)
        {
            i += n;
            Prefix = new string(' ', 4 * i);
        }

        public void Unindent(int n = 1)
        {
            i -= n;
            Prefix = new string(' ', 4 * i);
        }

        public void BeginScope()
        {
            AppendLine("{");
            Indent();
        }

        public void EndScope()
        {
            Unindent();
            AppendLine("}");
        }

        public Scope ScopeLine(string s = "", bool c = true)
        {
            if (!c)
            {
                return null;
            }

            AppendLine(s);
            return new Scope(this);
        }

        public void AppendLine(string s = "")
        {
            sb.AppendLine(Prefix + s);
        }

        public void AppendLines(string s)
        {
            foreach (var l in s.Split('\n'))
            {
                sb.AppendLine(Prefix + l);
            }
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
