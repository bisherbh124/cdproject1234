using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompiler2
{
    public class Parser
    {
        SymbolTable ST;
        int scopeCounter;
        Lexical lex;
        bool hfla;
        int indexarrynumber;
        int index;

        public Parser(string input)
        {
            lex = new Lexical(input);
            hfla = true;
            lex.genTokens();
            lex.printTokens();
            ST = new SymbolTable();
            scopeCounter = 0;
            indexarrynumber = 1;
            index = 0;
            checkGrammer();
        }

        public void checkGrammer()
        {
            if (StartProgram() == true)
            {
                Console.WriteLine("True");
            }
            else
            {
                Console.WriteLine("False");
            }
        }

        public bool StartProgram()
        {
        cc:
            if (lex.TL.tokens[index].value == "int")
            {
            ff: index++;
                if (lex.TL.tokens[index].value == "main")
                {
                dd: index++;
                    if (lex.TL.tokens[index].value == "(")
                    {
                    xx: index++;
                        if (lex.TL.tokens[index].value == ")")
                        {
                        cx: index++;
                            if (lex.TL.tokens[index].value == "{")
                            {
                                index++;
                                scopeCounter++;
                                if (StmtList())
                                {
                                    if (lex.TL.tokens[index].value == "}")
                                    {
                                        index++;
                                        ST.deleteSymbolsByScope(scopeCounter);
                                        Symbol.flag = false;
                                        scopeCounter--;
                                        return true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERRoR forget } ");
                                        lex.TL.tokens[index].value = "}";
                                        index--;
                                        goto cx;

                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("ERRoR forget { ");
                                lex.TL.tokens[index].value = "{";
                                index--;
                                goto cx;
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERRoR forget ) ");
                            lex.TL.tokens[index].value = ")";
                            index--;
                            goto xx;

                        }
                    }
                    else
                    {
                        Console.WriteLine("ERRoR forget ( ");
                        lex.TL.tokens[index].value = "(";
                        index--;
                        goto dd;
                    }
                }
                else
                {
                    Console.WriteLine("ERRoR must be main  ");

                    lex.TL.tokens[index].value = "main";
                    index--;
                    goto ff;
                }
            }
            else
            {

                Console.WriteLine("ERRoR must be int  ");
                lex.TL.tokens[index].value = "int";
                goto cc;

            }
            return false;
        }

        public bool StmtList()
        {
            if (Stmt())
            {
                if (lex.TL.tokens[index].value == "}")
                {
                    return true;
                }
                if (StmtList())
                {
                    return true;
                }
            }
            return false;
        }

        public bool Stmt()
        {
            if (lex.TL.tokens[index].value == "else")
            {
                return false;
            }
            else
            if (lex.TL.tokens[index].value == "cout")
            {
                if (OutputStmt())
                {
                    return true;
                }
            }
            else if (lex.TL.tokens[index].value == "cin")
            {
                if (InputStmt())
                {
                    return true;
                }
            }
            else if (lex.TL.tokens[index].type == Language.DataType)
            {

                if (DefinintionStmt())
                {
                    return true;
                }
                //definition
            }
            else if (lex.TL.tokens[index].type == Language.Identifer)

            //if (lex.TL.tokens[index].value == "(" || lex.TL.tokens[index].type == Language.Identifer || lex.TL.tokens[index].type == Language.IntNumber || lex.TL.tokens[index].type == Language.trueBoolean || lex.TL.tokens[index].type == Language.String || lex.TL.tokens[index].type == Language.Char)
            {
                if (Exp())
                {

                    if (lex.TL.tokens[index].value == ";")
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (lex.TL.tokens[index].value == "for" || lex.TL.tokens[index].value == "while")
            {
                if (Exprloop())
                {
                    return true;
                }
            }
            else if (lex.TL.tokens[index].value == "if")
            {
                if (ifs())
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public bool InputStmt()
        {
            if (lex.TL.tokens[index].value == "cin")
            {
                index++;
                if (lex.TL.tokens[index].value == ">")
                {
                    index++;
                    if (lex.TL.tokens[index].value == ">")
                    {
                        index++;
                        if (lex.TL.tokens[index].type == Language.Identifer)
                        {
                            index++;
                            if (loop2())
                            {
                                return true;
                            }
                        }

                    }
                }
            }
            return false;
        }
        public bool loop2()
        {
            if (lex.TL.tokens[index].value == ";")
            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == ">")
            {
                index++;
                if (lex.TL.tokens[index].value == ">")
                {
                    index++;
                    if (lex.TL.tokens[index].type == Language.Identifer)
                    {
                        index++;
                        if (loop2())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }
        public bool DefinintionStmt()
        {
            Token tmpTypeToken = lex.TL.tokens[index];
            if (lex.TL.tokens[index].type == Language.DataType)
            {
                index++;
                if (Ds2(tmpTypeToken.value))
                {
                    if (loopdec(tmpTypeToken.value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool loopdec(string tmptype)
        {
            if (lex.TL.tokens[index].value == ";")
            {
                index++;
                indexarrynumber = 1;
                return true;
            }
            else
                if (lex.TL.tokens[index].value == ",")
            {
                index++;
                if (Ds2(tmptype))
                {
                    if (loopdec(tmptype))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Ds2(string tmptype)
        {
            Token tmpIdToken = lex.TL.tokens[index];

            if (lex.TL.tokens[index].type == Language.Identifer)
            {
                Symbol s = new Symbol(tmpIdToken.value, tmptype, scopeCounter);
                if (!ST.searchSymbolByName(tmpIdToken.value))
                {


                    index++;
                    if (lex.TL.tokens[index].value == "=")
                    {
                        index++;
                        if (Exp3(tmptype))
                        {
                            ST.addSymbol(s);
                            ST.print();
                            return true;
                        }
                    }
                    else if (lex.TL.tokens[index].value == "[")
                    {
                        index++;
                        if (E(tmptype))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        ST.addSymbol(s);
                        ST.print();
                        return true;
                    }



                }
                else if (ST.searchSymbolbyName2(tmpIdToken.value, scopeCounter))
                {
                    if (!ST.searchSymbolbyName3(tmpIdToken.value, scopeCounter))
                    {
                        index++;
                        if (lex.TL.tokens[index].value == "=")
                        {
                            index++;
                            if (Exp3(tmptype))
                            {
                                ST.addSymbol(s);
                                ST.print();
                                return true;
                            }
                        }
                        else if (lex.TL.tokens[index].value == "[")
                        {
                            index++;
                            if (E(tmptype))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            ST.addSymbol(s);
                            ST.print();
                            return true;
                        }

                    }
                    else if (ST.searchSymbolbyName3(tmpIdToken.value, scopeCounter))
                    {

                        Console.WriteLine("ERRor in name");
                    }

                }
                else if (ST.searchSymbolByName(tmpIdToken.value))
                {

                    Console.WriteLine("ERRor in name");
                }
            }
            return false;
        }
        public bool E(string tmptype)
        {

            if (lex.TL.tokens[index].type == Language.IntNumber)
            {
                Token tmpTypeToken = lex.TL.tokens[index];
                var indexnumber = tmpTypeToken.value;
                index++;
                if (lex.TL.tokens[index].value == "]")
                {
                    index++;
                    if (lex.TL.tokens[index].value == "=")
                    {
                        index++;
                        if (ArrayIndex(tmptype, indexnumber))
                        {
                            return true;
                        }
                    }
                    else if (lex.TL.tokens[index].value == "[")
                    {
                        index++;
                        if (E(tmptype))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ifs()
        {
            if (lex.TL.tokens[index].value == "if")
            {
                index++;
                if (lex.TL.tokens[index].value == "(")
                {
                    index++;
                    if (Exp())
                    {
                        if (lex.TL.tokens[index].value == ")")
                        {
                            index++;
                            if (lex.TL.tokens[index].value == "{")
                            {
                                index++;

                                scopeCounter++;
                                if (ifs())
                                {
                                    if (lex.TL.tokens[index].value == "}")
                                    {
                                        index++;
                                        ST.deleteSymbolsByScope(scopeCounter);
                                        Symbol.flag = false;
                                        scopeCounter--;
                                        if (lex.TL.tokens[index].value == "else")
                                        {
                                            index++;
                                            if (lex.TL.tokens[index].value == "{")
                                            {
                                                index++;

                                                scopeCounter++;
                                                if (ifs())
                                                {
                                                    if (lex.TL.tokens[index].value == "}")
                                                    {
                                                        index++;
                                                        ST.deleteSymbolsByScope(scopeCounter);
                                                        Symbol.flag = false;
                                                        scopeCounter--;
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                        return true;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else if (StmtList())
            {
                return true;
            }
            return false;
        }


        public bool Exprloop()
        {
            if (lex.TL.tokens[index].value == "for" || lex.TL.tokens[index].value == "while")
            {
                if (lex.TL.tokens[index].value == "for")
                {
                    index++;
                    if (lex.TL.tokens[index].value == "(")
                    {
                        index++;
                        if (DefinintionStmt())
                        {
                            if (Exp())
                            {
                                if (lex.TL.tokens[index].value == ";")
                                {
                                    index++;
                                    if (Exp())
                                    {

                                        if (lex.TL.tokens[index].value == ")")
                                        {
                                            index++;
                                            if (lex.TL.tokens[index].value == "{")
                                            {
                                                index++;

                                                scopeCounter++;
                                                if (StmtList())
                                                {
                                                    if (lex.TL.tokens[index].value == "}")
                                                    {
                                                        index++;
                                                        ST.deleteSymbolsByScope(scopeCounter);
                                                        Symbol.flag = false;
                                                        scopeCounter--;
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    if (lex.TL.tokens[index].value == "while")
                {
                    index++;
                    if (lex.TL.tokens[index].value == "(")
                    {
                        index++;
                        if (Exp())
                        {
                            if (lex.TL.tokens[index].value == ")")
                            {
                                index++;
                                if (lex.TL.tokens[index].value == "{")
                                {
                                    index++;

                                    scopeCounter++;
                                    if (StmtList())
                                    {
                                        if (lex.TL.tokens[index].value == "}")
                                        {
                                            index++;
                                            ST.deleteSymbolsByScope(scopeCounter);
                                            Symbol.flag = false;
                                            scopeCounter--;
                                            return true;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            //else if (StmtList())
            //{
            //    return true;
            //}
            return false;
        }
        public bool Exp()
        {

            if (lex.TL.tokens[index].value == "(" || lex.TL.tokens[index].type == Language.Identifer || lex.TL.tokens[index].type == Language.IntNumber || lex.TL.tokens[index].type == Language.trueBoolean || lex.TL.tokens[index].type == Language.String || lex.TL.tokens[index].type == Language.Char)
            {

                if (SimpleExp())
                {
                    if (CompareOp())
                    {
                        if (SimpleExp())
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //index++;
                        return true;
                    }
                }
            }
            return false;

        }
        public bool CompareOp()
        {
            if (lex.TL.tokens[index].value == ">")
            {
                index++;
                return true;
            }
            else
            if (lex.TL.tokens[index].value == "<")

            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "==")

            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "=")

            {
                index++;
                return true;
            }

            else if (lex.TL.tokens[index].value == "!=")

            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "<=")

            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == ">=")

            {
                index++;
                return true;
            }
            return false;
        }
        public bool SimpleExp()
        {
            if (Term())
            {
                if (SimpleExpPrime())
                {
                    return true;
                }
            }
            return false;
        }
        public bool SimpleExpPrime()
        {
            if (AddOp())
            {
                if (Term())
                {
                    if (SimpleExpPrime())
                    {

                        return true;
                    }

                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public bool AddOp()
        {
            if (lex.TL.tokens[index].value == "+")
            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "-")
            {
                index++;
                return true;
            }
            return false;
        }
        public bool BooleanOp()
        {
            if (lex.TL.tokens[index].value == "&&")
            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "||")
            {
                index++;
                return true;
            }
            return false;
        }
        public bool Term()
        {

            if (Factor())
            {
                if (TermPrime())
                {
                    return true;
                }
            }
            return false;
        }
        public bool TermPrime()
        {
            if (MulOp())
            {
                if (Factor())
                {
                    if (TermPrime())
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public bool MulOp()
        {
            if (lex.TL.tokens[index].value == "*")
            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "/")
            {
                index++;
                return true;
            }
            return false;
        }
        public bool Factor()
        {
            if (Factor2())
            {
                return true;
            }
            else if (lex.TL.tokens[index].value == "-")
            {
                index++;
                if (Factor2())
                {
                    return true;
                }
            }
            else
               if (Boolean())
            {
                return true;
            }
            return false;
        }
        public bool Boolean()
        {
            if (lex.TL.tokens[index].type == Language.trueBoolean)
            {
                index++;
                return true;
            }
            else
              if (lex.TL.tokens[index].type == Language.falseBoolean)
            {
                index++;
                return true;
            }
            return false;

        }

        public bool Factor2()
        {
            if (lex.TL.tokens[index].value == "(")
            {
                index++;
                if (SimpleExp())
                {
                    if (lex.TL.tokens[index].value == ")")
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (Values() != "")
            {
                if (true)
                {
                    return true;
                }
            }
            else if (lex.TL.tokens[index].type == Language.Identifer)
            {
                index++;
                return true;
            }
            else { return false; }
            return false;
        }


        public bool Exp3(String tmptype)
        {

            if (lex.TL.tokens[index].value == "(" || lex.TL.tokens[index].type == Language.Identifer || lex.TL.tokens[index].type == Language.IntNumber || lex.TL.tokens[index].type == Language.trueBoolean || lex.TL.tokens[index].type == Language.String || lex.TL.tokens[index].type == Language.Char)
            {
                if (SimpleExp2(tmptype))
                {



                    return true;




                }
            }
            return false;

        }

        public bool SimpleExp2(String tmptype)
        {
            if (Term2(tmptype))
            {
                if (SimpleExpPrime2(tmptype))
                {
                    return true;
                }
            }
            return false;
        }
        public bool SimpleExpPrime2(string tmptype)
        {
            if (AddOp2())
            {
                if (Term2(tmptype))
                {
                    if (SimpleExpPrime2(tmptype))
                    {

                        return true;
                    }

                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public bool AddOp2()
        {
            if (lex.TL.tokens[index].value == "+")
            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "-")
            {
                index++;
                return true;
            }
            return false;
        }

        public bool Term2(string tmptype)
        {
            if (Factor2a(tmptype))
            {
                if (TermPrime2(tmptype))
                {
                    return true;
                }
            }
            return false;
        }
        public bool TermPrime2(string tmptype)
        {
            if (MulOp2())
            {
                if (Factor2a(tmptype))
                {
                    if (TermPrime2(tmptype))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public bool MulOp2()
        {
            if (lex.TL.tokens[index].value == "*")
            {
                index++;
                return true;
            }
            else if (lex.TL.tokens[index].value == "/")
            {
                index++;
                return true;
            }
            return false;
        }
        public bool Factor2a(string tmptype)
        {
            if (Factor4(tmptype))
            {
                return true;
            }
            else if (lex.TL.tokens[index].value == "-")
            {
                index++;
                if (Factor4(tmptype))
                {
                    return true;
                }
            }
            else
               if (Boolean2())
            {
                return true;
            }
            return false;
        }
        public bool Boolean2()
        {
            if (lex.TL.tokens[index].type == Language.trueBoolean)
            {
                index++;
                return true;
            }
            else
              if (lex.TL.tokens[index].type == Language.falseBoolean)
            {
                index++;
                return true;
            }
            return false;

        }
        public bool Factor4(string tmptype)
        {
            if (lex.TL.tokens[index].value == "(")
            {
                index++;
                if (SimpleExp2(tmptype))
                {
                    if (lex.TL.tokens[index].value == ")")
                    {
                        index++;
                        return true;
                    }
                }
            }
            else
            {

                string valRes = Values();

                if (valRes != "")
                {
                    if (valRes == tmptype)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error in type");
                    }
                }
                else
                if (lex.TL.tokens[index].type == Language.Identifer)
                {
                    index++;
                    return true;
                }
            }


            return false;
        }
        public bool ArrayIndex(string tmptype, string indexnumber)
        {
            if (lex.TL.tokens[index].value == "{")
            {
                index++;
                if (Element(tmptype, indexnumber))
                {
                    if (lex.TL.tokens[index].value == "}")
                    {
                        index++;
                        return true;
                    }
                }

                if (lex.TL.tokens[index].value == "}")
                {
                    index++;
                    return true;
                }
            }
            return false;
        }
        public bool Element(string tmptype, string indexnumber)
        {
            if (!(indexarrynumber > Convert.ToInt32(indexnumber)))
            {
                if (ArryVal(tmptype, indexnumber))
                {

                    if (lex.TL.tokens[index].value == ",")
                    {
                        indexarrynumber++;
                        index++;
                        if (Element(tmptype, indexnumber))
                        {
                            return true;
                        }

                    }
                    return true;
                }

            }
            else
            {
                Console.WriteLine("ERRoR in index ARRy");
                return false;
            }

            return false;
        }
        public bool ArryVal(string tmptype, string indexnumber)
        {
            if (ArrayIndex(tmptype, indexnumber))
            {
                return true;
            }
            else
            {
                String val = Values();
                if (val != "")
                {
                    if (val == tmptype)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("ERROR in type of value in index arry");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool Exp2()
        {

            if (lex.TL.tokens[index].value == "(" || lex.TL.tokens[index].type == Language.Identifer || lex.TL.tokens[index].type == Language.IntNumber || lex.TL.tokens[index].type == Language.trueBoolean || lex.TL.tokens[index].type == Language.falseBoolean || lex.TL.tokens[index].type == Language.String || lex.TL.tokens[index].type == Language.Char)
            {
                if (SimpleExp())
                {


                    return true;


                }
            }
            return false;

        }
        public bool OutputStmt()
        {
            if (lex.TL.tokens[index].value == "cout")
            {
                index++;
                if (lex.TL.tokens[index].value == "<<")
                {

                    index++;
                    if (Exp2())
                    {
                        if (Loop())
                        {
                            return true;
                        }

                    }

                }

            }
            return false;
        }
        public bool Loop()
        {
            if (lex.TL.tokens[index].value == ";")
            {
                index++;
                return true;
            }
            else

                if (lex.TL.tokens[index].value == "<<")
            {


                index++;
                if (Exp2())
                {
                    if (Loop())
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        public String Values()
        {
            if (lex.TL.tokens[index].type == Language.IntNumber)
            {
                index++;
                return "int";
            }

            else if (lex.TL.tokens[index].type == Language.DoubleNumber)
            {
                index++;
                return "double";
            }
            else if (lex.TL.tokens[index].type == Language.falseBoolean)
            {
                index++;
                return "bool";
            }
            else if (lex.TL.tokens[index].type == Language.trueBoolean)
            {
                index++;
                return "bool";
            }
            else if (lex.TL.tokens[index].type == Language.String)
            {
                index++;
                return "string";
            }
            else if (lex.TL.tokens[index].type == Language.Char)
            {
                index++;
                return "char";
            }
            else
            {
                return "";
            }
        }
    }
}