
using System.Collections;
using System.Collections.Generic;


public class Generator : SingletonMonobehaviour<Generator>
{
    public int RandomNumber(int min=0, int max=10)
    {
        System.Random rnd = new System.Random();
        int num = rnd.Next(min, max);

        return num;
    }
}
