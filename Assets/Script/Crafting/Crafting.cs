using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting
{
    private static Crafting instance;
    public static Crafting Instance
    {
        get
        {
            if(instance == null)
                instance = new Crafting();
            return instance;
        }
    }

    //recipes 는 일단 있는데 사용처는 없음 그냥 필요하면 쓸듯 (어느 조건에 해당한 조합법을 모아놓고 보여준다던가)
    readonly List<Recipe> recipes = new ();
    readonly List<Recipe> recipes3X3 = new ();
    readonly List<Recipe> recipes3X2 = new ();
    readonly List<Recipe> recipes2X3 = new ();
    readonly List<Recipe> recipes2X2 = new ();
    readonly List<Recipe> recipes3X1 = new ();
    readonly List<Recipe> recipes1X3 = new ();
    readonly List<Recipe> recipes2X1 = new ();
    readonly List<Recipe> recipes1X2 = new ();
    readonly List<Recipe> recipes1X1 = new ();

    private Crafting()
    {
        AddRecipe(new Plank_Recipe());
        AddRecipe(new WoodenStick_Recipe());
        AddRecipe(new Workbench_Recipe());
    }

    private void AddRecipe(Recipe r)
    {
        if (r.recipe is Table3X3)
            recipes3X3.Add(r);
        else if (r.recipe is Table3X2)
            recipes3X2.Add(r);
        else if (r.recipe is Table2X3)
            recipes2X3.Add(r);
        else if (r.recipe is Table2X2)
            recipes2X2.Add(r);
        else if (r.recipe is Table3X1)
            recipes3X1.Add(r);
        else if (r.recipe is Table1X3)
            recipes1X3.Add(r);
        else if (r.recipe is Table2X1)
            recipes2X1.Add(r);
        else if (r.recipe is Table1X2)
            recipes1X2.Add(r);
        else if (r.recipe is Table1X1)
            recipes1X1.Add(r);
    }

    public void Combination(Table table, out int id, out int result)
    {
        id = 0;
        result = 0;
        if (table.codes == null)
            return;
        if (table is Table3X3)
        {
            for (int i = 0; i < recipes3X3.Count; i++)
            {
                if (recipes3X3[i].Comparison(table))
                {
                    id = recipes3X3[i].result;
                    result = recipes3X3[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table3X2)
        {
            for (int i = 0; i < recipes3X2.Count; i++)
            {
                if (recipes3X2[i].Comparison(table))
                {
                    id = recipes3X2[i].result;
                    result = recipes3X2[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table2X3)
        {
            for (int i = 0; i < recipes2X3.Count; i++)
            {
                if (recipes2X3[i].Comparison(table))
                {
                    id = recipes2X3[i].result;
                    result = recipes2X3[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table2X2)
        {
            for (int i = 0; i < recipes2X2.Count; i++)
            {
                if (recipes2X2[i].Comparison(table))
                {
                    id = recipes2X2[i].result;
                    result = recipes2X2[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table3X1)
        {
            for (int i = 0; i < recipes3X1.Count; i++)
            {
                if (recipes3X1[i].Comparison(table))
                {
                    id = recipes3X1[i].result;
                    result = recipes3X1[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table2X2)
        {
            for (int i = 0; i < recipes1X3.Count; i++)
            {
                if (recipes1X3[i].Comparison(table))
                {
                    id = recipes1X3[i].result;
                    result = recipes1X3[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table2X1)
        {
            for (int i = 0; i < recipes2X1.Count; i++)
            {
                if (recipes2X1[i].Comparison(table))
                {
                    id = recipes2X1[i].result;
                    result = recipes2X1[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table1X2)
        {
            for (int i = 0; i < recipes1X2.Count; i++)
            {
                if (recipes1X2[i].Comparison(table))
                {
                    id = recipes1X2[i].result;
                    result = recipes1X2[i].resultCount;
                    return;
                }
            }
        }
        else if (table is Table1X1)
        {
            for (int i = 0; i < recipes1X1.Count; i++)
            {
                if (recipes1X1[i].Comparison(table))
                {
                    id = recipes1X1[i].result;
                    result = recipes1X1[i].resultCount;
                    return;
                }
            }
        }
    }
}

public class Plank_Recipe : Recipe
{
    public Plank_Recipe()
    {
        result = 9;
        resultCount = 4;

        recipe = new()
        {
            codes = new int[3,3]
            {
                { 8, 0, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            }
        };
        recipe.Slice(recipe, out recipe);
    }
}
public class WoodenStick_Recipe : Recipe
{
    public WoodenStick_Recipe()
    {
        result = 10;
        resultCount = 4;

        recipe = new()
        {
            codes = new int[3, 3]
            {
                { 9, 0, 0 },
                { 9, 0, 0 },
                { 0, 0, 0 }
            }
        };
        recipe.Slice(recipe, out recipe);
    }
}
public class Workbench_Recipe : Recipe
{
    public Workbench_Recipe()
    {
        result = 6;
        resultCount = 1;

        recipe = new()
        {
            codes = new int[3, 3]
            {
                { 9, 9, 0 },
                { 9, 9, 0 },
                { 0, 0, 0 }
            }
        };
        recipe.Slice(recipe, out recipe);
    }
}

public class Recipe
{
    public Table recipe;
    public int result;
    public int resultCount;

    public bool Comparison(Table x3)
    {
        return recipe._Equals(x3);
    }
}

public class Table
{
    public int[,] codes;

    public bool _Equals(Table table)
    {
        if (table.codes.GetLength(0) != codes.GetLength(0) || table.codes.GetLength(1) != codes.GetLength(1))
            return false;

        for (int i = 0; i < table.codes.GetLength(0); i++)
        {
            for (int j = 0; j < table.codes.GetLength(1); j++)
            {
                if (table.codes[i, j] != codes[i, j])
                    return false;
            }
        }

        return true;
    }

    public Table NewTable(int x, int y)
    {
        if (x == 3 && y == 3) return new Table3X3();
        else if (x == 3 && y == 2) return new Table3X2();
        else if (x == 2 && y == 3) return new Table2X3();
        else if (x == 2 && y == 2) return new Table2X2();
        else if (x == 2 && y == 1) return new Table2X1();
        else if (x == 1 && y == 2) return new Table1X2();
        else if (x == 1 && y == 1) return new Table1X1();

        return new Table();
    }

    public void Slice(Table table, out Table slice)
    {
        int rowfront = 0;
        int rowback = table.codes.GetLength(0);
        int linefront = 0;
        int lineback = table.codes.GetLength(1);

        bool forward = false;
        bool reverse = false;

        for (int i = 0; i < table.codes.GetLength(0); i++)
        {
            rowfront = i;
            for (int j = 0; j < table.codes.GetLength(1); j++)
            {
                if (table.codes[i, j] != 0)
                {
                    forward = true;
                    break;
                }
            }
            if (forward)
                break;
        }

        //모든칸이 -1인경우
        if(rowfront == table.codes.GetLength(0) - 1 && forward == false)
        {
            slice = new Table();
            return;
        }

        for (int i = table.codes.GetLength(0) - 1; i >= 0; i--)
        {
            rowback = i;
            for (int j = 0; j < table.codes.GetLength(1); j++)
            {
                if (table.codes[i, j] != 0)
                {
                    reverse = true;
                    break;
                }
            }
            if (reverse)
                break;
        }

        //rowfront 가 시작줄 rowback 이 마지막 줄 rowfront ~ rowback 가 행의 크기

        forward = false; reverse = false;


        for (int i = 0; i < table.codes.GetLength(1); i++)
        {
            linefront = i;
            for (int j = 0; j < table.codes.GetLength(0); j++)
            {
                if (table.codes[j, i] != 0)
                {
                    forward = true;
                    break;
                }
            }
            if (forward)
                break;
        }

        for (int i = table.codes.GetLength(1) - 1; i >= 0; i--)
        {
            lineback = i;
            for (int j = 0; j < table.codes.GetLength(0); j++)
            {
                if (table.codes[j, i] != 0)
                {
                    reverse = true;
                    break;
                }
            }
            if (reverse)
                break;
        }

        int line = Mathf.Abs(linefront - lineback) + 1;
        int row  = Mathf.Abs(rowfront - rowback) + 1;
        slice = NewTable(row, line);
        for(int i = rowfront; i <= rowback; i++)
        {
            for(int j = linefront; j <= lineback; j++)
            {
                slice.codes[i - rowfront, j - linefront] = table.codes[i, j];
            }
        }
    }
}

public class Table3X3 : Table
{
    public Table3X3()
    {
        codes = new int[3, 3]
        {
            { 0, 0, 0},
            { 0, 0, 0},
            { 0, 0, 0}
        };
    }
}

public class Table2X3 : Table
{
    public Table2X3()
    {
        codes = new int[2, 3]
        {
            { 0, 0, 0},
            { 0, 0, 0}
        };
    }
}

public class Table3X2 : Table
{
    public Table3X2()
    {
        codes = new int[3, 2]
        {
            { 0, 0},
            { 0, 0},
            { 0, 0}
        };
    }
}

public class Table2X2 : Table
{
    public Table2X2()
    {
        codes = new int[2, 2]
        {
            { 0 , 0},
            { 0, 0},
        };
    }
}

public class Table3X1 : Table
{
    public Table3X1()
    {
        codes = new int[3, 1]
        {
            { 0 },
            { 0 },
            { 0 }
        };
    }
}

public class Table1X3 : Table
{
    public Table1X3()
    {
        codes = new int[1, 3]
        {
            {0, 0, 0}
        };
    }
}

public class Table1X2 : Table
{
    public Table1X2()
    {
        codes = new int[1, 2]
        {
            {0,0 }
        };
    }
}

public class Table2X1 : Table
{
    public Table2X1()
    {
        codes = new int[2, 1]
        {
            { 0},
            {0 }
        };
    }
}

public class Table1X1 : Table
{
    public Table1X1()
    {
        codes = new int[1, 1] 
        { { 0 } };
    }
}