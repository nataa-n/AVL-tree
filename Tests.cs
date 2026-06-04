using Xunit;

public class Tests
{
    // to start from the same tree
    private AVLTree<int> MakeSampleTree()
    {
        var tree = new AVLTree<int>();
        foreach (int v in new[] { 30, 20, 40, 10, 25, 35, 50 })
            tree.Insert(v);
        return tree;
    }


    [Theory]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void Search_FindsEveryInsertedValue(int value)
    {
        var tree = MakeSampleTree();
        Assert.True(tree.Search(value));
    }

    [Fact]
    public void Search_DoesNotFindMissingValue()
    {
        var tree = MakeSampleTree();
        Assert.False(tree.Search(99));
    }


    [Fact]
    public void InOrder_ReturnsValuesInSortedOrder()
    {
        var tree = MakeSampleTree();

        List<int> expected = new List<int> { 10, 20, 25, 30, 35, 40, 50 };
        Assert.Equal(expected, tree.InOrder());
    }


    [Fact]
    public void Insert_KeepsTreeBalanced_OnSortedInput()
    {
        var tree = new AVLTree<int>();
        for (int i = 1; i <= 1000; i++)
            tree.Insert(i); // sorted order is the worst case for a plain BST

        Assert.True(tree.IsBalanced());
    }


    [Fact]
    public void Delete_Leaf_RemovesItAndStaysBalanced()
    {
        var tree = MakeSampleTree();
        tree.Delete(10); // 10 is a leaf

        Assert.False(tree.Search(10));
        Assert.True(tree.IsBalanced());
    }

    [Fact]
    public void Delete_NodeWithTwoChildren_RemovesItAndStaysBalanced()
    {
        var tree = MakeSampleTree();
        tree.Delete(40); // 40 has two children

        Assert.False(tree.Search(40));
        Assert.True(tree.Search(35)); // a neighbour is still there
        Assert.True(tree.IsBalanced());
    }

    [Fact]
    public void Delete_Root_RemovesItAndStaysBalanced()
    {
        var tree = MakeSampleTree();
        tree.Delete(30); // 30 is the root

        Assert.False(tree.Search(30));
        Assert.True(tree.IsBalanced());
    }


    [Fact]
    public void Tree_WorksWithStrings()
    {
        var tree = new AVLTree<string>();
        foreach (string s in new[] { "mango", "apple", "pear", "banana", "kiwi" })
            tree.Insert(s);

        Assert.True(tree.Search("kiwi"));
        Assert.False(tree.Search("grape"));

        List<string> expected = new List<string> { "apple", "banana", "kiwi", "mango", "pear" };
        Assert.Equal(expected, tree.InOrder()); // strings come out alphabetical
    }

    [Fact]
    public void Height_StaysLogarithmic_OnSortedInput()
    {
        var tree = new AVLTree<int>();
        int n = 100000;
        for (int i = 1; i <= n; i++) 
        tree.Insert(i);
        Assert.True(tree.GetHeight() < 2 * Math.Log(n, 2));
    }

}