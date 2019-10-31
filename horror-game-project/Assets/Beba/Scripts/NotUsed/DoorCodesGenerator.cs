using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    [System.Serializable]
    public struct DoorCodesLists
    {
        public string theCode;
        public string codeName;
        public string[] hint;

        public DoorCodesLists(string c, string cname, string[] h)
        {
            theCode = c;
            codeName = cname;
            hint = h;
        }
    }


    public class DoorCodesGenerator : MonoBehaviour
    {
        public int codeLength;
        public int noOfCodesToGenerate;

        string codeName;
        string codeGenerated;
        string[] hintTemp;
        int index;

        DoorCodesLists newCode;

        public List<DoorCodesLists> codeLists = new List<DoorCodesLists>();

        private void Start()
        {
            index = 0;

            if (codeLength % 2 != 0)
            {
                Debug.Log("Code Length have to be in even number. i.e: 2,4,6 etc.");
                return;
            }

            for (int i = 0; i < noOfCodesToGenerate; i++)
            {
                hintTemp = new string[codeLength / 2];
                index++;

                codeName = "Code" + index;
                codeGenerated = CodeGenerator();
                newCode = new DoorCodesLists(codeGenerated, codeName, hintTemp);

                codeLists.Add(newCode);
            }

        }


        public string CodeGenerator()
        {
            bool isValid = false;
            int codeChunk;
            int index = 0;
            string theCodeTemp = "";

            for (int i = 0; i < codeLength / 2 - 1; i++)
            {

                while (!isValid && index < codeLength / 2)
                {
                    int no1 = RandomNumber();
                    int no2 = RandomNumber();

                    codeChunk = no1 * no2;

                    if (!ContainsZeroChecker(codeChunk))
                    {
                        if (codeChunk > 10 && codeChunk < 100)
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
            if (num == 0) { return true; }

            while (num > 0)
            {
                if (num % 10 == 0) { return true; }
                num /= 10;
            }

            return false;
        }
    }

}


