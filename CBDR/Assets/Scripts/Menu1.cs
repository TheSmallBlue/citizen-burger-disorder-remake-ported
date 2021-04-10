using System;
using System.Collections.Generic;
using UnityEngine;

public static class Menu
{
    public static Material GetFoodMaterial(string food)
    {
        return Resources.Load("UI/Materials/" + food) as Material;
    }

    public static float ScoreFood(string foodItem, Food foodToCompare)
    {
        float num = 0f;
        float num2 = 0f;
        if (foodToCompare.transform.Find("burger-bottom"))
        {
            List<Food> list = new List<Food>(foodToCompare.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger);
            Food.FoodType[] array;
            if (foodItem != null)
            {
                /*if (<> f__switch$map2 == null)
				{
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
                    dictionary.Add("Citizen", 0);
                    dictionary.Add("Family", 1);
                    dictionary.Add("Worker", 2);
                    dictionary.Add("President", 3);
                    dictionary.Add("Mayor", 4);
                    dictionary.Add("Boss", 5);
                    <> f__switch$map2 = dictionary;
                }
                int num3;
                if (<> f__switch$map2.TryGetValue(foodItem, ref num3))
				{
                    switch (num3)
                    {
                        case 0:
                            num2 = 4f;
                            array = new Food.FoodType[Citizen.Length - 1];
                            Array.Copy(Citizen, array, Citizen.Length - 1);
                            goto IL_23A;
                        case 1:
                            num2 = 8f;
                            array = new Food.FoodType[Family.Length - 1];
                            Array.Copy(Family, array, Family.Length - 1);
                            goto IL_23A;
                        case 2:
                            num2 = 6f;
                            array = new Food.FoodType[Worker.Length - 1];
                            Array.Copy(Worker, array, Worker.Length - 1);
                            goto IL_23A;
                        case 3:
                            num2 = 5f;
                            array = new Food.FoodType[President.Length - 1];
                            Array.Copy(President, array, President.Length - 1);
                            goto IL_23A;
                        case 4:
                            num2 = 5f;
                            array = new Food.FoodType[Mayor.Length - 1];
                            Array.Copy(Mayor, array, Mayor.Length - 1);
                            goto IL_23A;
                        case 5:
                            num2 = 6f;
                            array = new Food.FoodType[Boss.Length - 1];
                            Array.Copy(Boss, array, Boss.Length - 1);
                            goto IL_23A;
                    }
                }*/
            }
            num2 = 4f;
            array = new Food.FoodType[Citizen.Length - 1];
            Array.Copy(Citizen, array, Citizen.Length - 1);
            //IL_23A:
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Food food = list[i];
                for (int j = 0; j < array.Length; j++)
                {
                    if (food.type == array[j])
                    {
                        Debug.Log(string.Concat(new object[]
                        {
                            food.type,
                            "[",
                            i,
                            "] matches GF[",
                            j,
                            "]"
                        }));
                        float num4 = 1f;
                        array.SetValue(null, j);
                        Food.FoodType type = food.type;
                        if (type != Food.FoodType.tomato)
                        {
                            if (type != Food.FoodType.bacon)
                            {
                                if (type != Food.FoodType.patty)
                                {
                                    num += 1f * num4;
                                    num *= 1f - food.cooked;
                                    num *= 1f - food.overcooked;
                                }
                                else
                                {
                                    num += 2f * num4;
                                    if (food.cooked < 0.8f)
                                    {
                                        num *= food.cooked;
                                    }
                                    num *= 1f - food.overcooked;
                                }
                            }
                            else
                            {
                                num += 0.5f * num4;
                                if (food.cooked < 0.8f)
                                {
                                    num *= food.cooked;
                                }
                                num *= 1f - food.overcooked;
                            }
                        }
                        else
                        {
                            num += 0.5f * num4;
                            num *= 1f - food.cooked;
                            num *= 1f - food.overcooked;
                        }
                        list.Remove(food);
                        break;
                    }
                }
            }
            foreach (Food food2 in list)
            {
                if (food2.type != Food.FoodType.topBun)
                {
                    num -= 1f;
                }
            }
        }
        num = Mathf.Round(num);
        Debug.Log(string.Concat(new object[]
        {
            "This food scores ",
            num,
            " out of ",
            num2
        }));
        return num;
    }

    // Token: 0x060001A4 RID: 420 RVA: 0x00016ED0 File Offset: 0x000150D0
    public static bool CompareAgainstFood(string foodItem, Food foodToCompare)
    {
        bool result = true;
        if (foodItem != null)
        {
            /*if (<> f__switch$map3 == null)
			{
                Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
                dictionary.Add("Citizen", 0);
                dictionary.Add("Family", 1);
                dictionary.Add("Worker", 2);
                dictionary.Add("President", 3);
                dictionary.Add("Mayor", 4);
                dictionary.Add("Boss", 5);
                <> f__switch$map3 = dictionary;
            }
            int num;
            if (<> f__switch$map3.TryGetValue(foodItem, ref num))
			{
                switch (num)
                {
                    case 0:
                        if (foodToCompare.type == Food.FoodType.bun)
                        {
                            List<Food> foodOnBurger = foodToCompare.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger;
                            if (foodOnBurger.Count == Citizen.Length)
                            {
                                for (int i = 0; i < foodOnBurger.Count; i++)
                                {
                                    if (foodOnBurger[i].type != Citizen[i])
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 1:
                        if (foodToCompare.type == Food.FoodType.bun)
                        {
                            List<Food> foodOnBurger2 = foodToCompare.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger;
                            if (foodOnBurger2.Count == Family.Length)
                            {
                                for (int j = 0; j < foodOnBurger2.Count; j++)
                                {
                                    if (foodOnBurger2[j].type != Family[j])
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 2:
                        if (foodToCompare.type == Food.FoodType.bun)
                        {
                            List<Food> foodOnBurger3 = foodToCompare.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger;
                            if (foodOnBurger3.Count == Worker.Length)
                            {
                                for (int k = 0; k < foodOnBurger3.Count; k++)
                                {
                                    if (foodOnBurger3[k].type != Worker[k])
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 3:
                        if (foodToCompare.type == Food.FoodType.bun)
                        {
                            List<Food> foodOnBurger4 = foodToCompare.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger;
                            if (foodOnBurger4.Count == President.Length)
                            {
                                for (int l = 0; l < foodOnBurger4.Count; l++)
                                {
                                    if (foodOnBurger4[l].type != President[l])
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 4:
                        if (foodToCompare.type == Food.FoodType.bun)
                        {
                            List<Food> foodOnBurger5 = foodToCompare.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger;
                            if (foodOnBurger5.Count == Mayor.Length)
                            {
                                for (int m = 0; m < foodOnBurger5.Count; m++)
                                {
                                    if (foodOnBurger5[m].type != Mayor[m])
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 5:
                        if (foodToCompare.type == Food.FoodType.bun)
                        {
                            List<Food> foodOnBurger6 = foodToCompare.transform.Find("burger-bottom").Find("triggerBunStack").GetComponent<BurgerStacking>().foodOnBurger;
                            if (foodOnBurger6.Count == Boss.Length)
                            {
                                for (int n = 0; n < foodOnBurger6.Count; n++)
                                {
                                    if (foodOnBurger6[n].type != Boss[n])
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                }
            }*/
        }
        return result;
    }

    // Token: 0x04000208 RID: 520
    public static Food.FoodType[] Citizen = new Food.FoodType[]
    {
        Food.FoodType.patty,
        Food.FoodType.cheese,
        Food.FoodType.lettuce,
        Food.FoodType.topBun
    };

    // Token: 0x04000209 RID: 521
    public static Food.FoodType[] Family = new Food.FoodType[]
    {
        Food.FoodType.patty,
        Food.FoodType.cheese,
        Food.FoodType.bun,
        Food.FoodType.patty,
        Food.FoodType.cheese,
        Food.FoodType.lettuce,
        Food.FoodType.topBun
    };

    // Token: 0x0400020A RID: 522
    public static Food.FoodType[] Worker = new Food.FoodType[]
    {
        Food.FoodType.patty,
        Food.FoodType.cheese,
        Food.FoodType.patty,
        Food.FoodType.cheese,
        Food.FoodType.topBun
    };

    // Token: 0x0400020B RID: 523
    public static Food.FoodType[] President = new Food.FoodType[]
    {
        Food.FoodType.cheese,
        Food.FoodType.patty,
        Food.FoodType.lettuce,
        Food.FoodType.tomato,
        Food.FoodType.tomato,
        Food.FoodType.topBun
    };

    // Token: 0x0400020C RID: 524
    public static Food.FoodType[] Mayor = new Food.FoodType[]
    {
        Food.FoodType.patty,
        Food.FoodType.lettuce,
        Food.FoodType.tomato,
        Food.FoodType.tomato,
        Food.FoodType.bacon,
        Food.FoodType.bacon,
        Food.FoodType.topBun
    };

    // Token: 0x0400020D RID: 525
    public static Food.FoodType[] Boss = new Food.FoodType[]
    {
        Food.FoodType.patty,
        Food.FoodType.cheese,
        Food.FoodType.patty,
        Food.FoodType.bacon,
        Food.FoodType.bacon,
        Food.FoodType.topBun
    };

    // Token: 0x0400020E RID: 526
    public static Food.FoodType[][] Items = new Food.FoodType[][]
    {
        Citizen,
        Family,
        Worker,
        President,
        Mayor,
        Boss
    };

    // Token: 0x0400020F RID: 527
    public static string[] ItemNames = new string[]
    {
        "Citizen",
        "Family",
        "Worker",
        "President",
        "Mayor",
        "Boss"
    };
}
