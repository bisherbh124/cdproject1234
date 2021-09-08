using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompiler2
{
    class Program
    {
        /*

       */

        static void Main(string[] args)
        {
            Parser pars = new Parser(@"int main(){int x; int y; while(x>5){cout<<'number your entred is'<<x;if(y>10){x=x-1;}}}");


           

        }
    }
}
