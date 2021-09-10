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
            Parser pars = new Parser(@"class amjad {
int maissn(int x,int y){}
int main(){
maissn(1,);
}
}");
        }
    }
}
