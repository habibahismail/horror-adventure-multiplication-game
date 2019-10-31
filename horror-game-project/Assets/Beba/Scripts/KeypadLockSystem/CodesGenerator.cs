using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    [System.Serializable]
    public class CodesLists
    {
        public string theCode;
        public string codeName;
        public string[] hint;
        public bool isSolved;

        public CodesLists(string c, string cname, string[] h, bool s)
        {
            theCode = c;
            codeName = cname;
            hint = h;
            isSolved = s;
        }
        
    }

    public class CodesGenerator
    {
        int codeLength;
        int noOfCodesToGenerate;

        protected string codeName;
        protected string codeGenerated;
        protected string[] hintTemp;
        protected int index;

        protected static CodesLists newCode;

        public List<CodesLists> codeLists = new List<CodesLists>();

        public CodesGenerator() {

            GameEvents.SaveInitiated += Save;
        }

        public CodesGenerator(int length, int toGenerate)
        {
            index = 0;
            codeLength = length;
            noOfCodesToGenerate = toGenerate;

            if(codeLength % 2 != 0 && codeLength != 0)
            {
                Debug.Log("Code Length have to be in even number. i.e: 2,4,6 etc.");
            }

            for (int i = 0; i < noOfCodesToGenerate; i++)
            {
                hintTemp = new string[codeLength / 2];
                index++;

                codeName = "Code" + index;
                codeGenerated = CodeGenerator();
                newCode = new CodesLists(codeGenerated, codeName, hintTemp, false);
                
                codeLists.Add(newCode);
            }

            GameEvents.SaveInitiated += Save;
        }


        private string CodeGenerator()
        {
            bool isValid = false;
            int codeChunk;
            int index = 0;
            string theCodeTemp = "";
            
            for (int i = 0; i < codeLength/2 - 1; i++)
            {
                while (!isValid && index < codeLength/2)
                {
                    int no1 = RandomNumber();
                    int no2 = RandomNumber();
                    
                    codeChunk = no1 * no2;

                    if(!ContainsZeroChecker(codeChunk))
                    {
                        if(codeChunk > 10 && codeChunk < 100 )
                        {
                            isValid = true;
                            theCodeTemp = theCodeTemp + codeChunk;
                            hintTemp[index] = no1 + " x " + no2;
                            
                            index++;
                        }
                    }

                    isValid = false;
                }
                
            }
            return theCodeTemp;
        }

        private int RandomNumber()
        {
            int number = Random.Range(1, 10);
            return number;
        }

        private bool ContainsZeroChecker(int num)
        {
            if(num == 0) { return true; }

            while (num > 0)
            {
                if(num % 10 == 0) { return true; }
                num /= 10;
            }

            return false;
        }

        void ReloadCodeLists(List<CodesLists> codes)
        {
            foreach (CodesLists code in codes)
            {
                codeLists.Add(code);
            }
        }

        void Save()
        {
            SaveLoad.Save<List<CodesLists>>(codeLists, "DoorCodes");
        }

        public List<CodesLists> Load()
        {
            if (SaveLoad.SaveExist("DoorCodes"))
            {
                ReloadCodeLists(SaveLoad.Load<List<CodesLists>>("DoorCodes"));
            }

            return codeLists;
        }

        
    }
}

