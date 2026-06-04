public class Node<T>
{
    public T Value; // data that node stores
    public Node<T> Left; // reference to the left child
    public Node<T> Right; // reference to the right child
    public int Height; // height of the subtree rooted at this node

    public Node(T value)
    {
        Value = value;
        Left = null;
        Right = null;
        Height = 1;
    }
}

public class AVLTree<T> where T : IComparable<T>
{
    private Node<T> root; 
    private int Height(Node<T> node)
    {
        if (node == null)
            return 0;
        return node.Height;
    }

    private void UpdateHeight(Node<T> node)
    {
        int leftHeight = Height(node.Left);
        int rightHeight = Height(node.Right);
        node.Height = 1 + Math.Max(leftHeight, rightHeight);
    }
    private int BalanceFactor(Node<T> node)
    {
        if (node == null)
            return 0;
        return Height(node.Left) - Height(node.Right);
    }
    private Node<T> RotateRight(Node<T> y)
    {
        Node<T> x  = y.Left;
        Node<T> S2 = x.Right;

        x.Right = y;
        y.Left  = S2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x; // new root of subtree
    }
    private Node<T> RotateLeft(Node<T> x)
    {
        Node<T> y  = x.Right;
        Node<T> S2 = y.Left;

        y.Left  = x;
        x.Right = S2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }
    
    // public entry point
    public void Insert(T value)
    {
        root = Insert(root, value);
    }

    // inserts value into the subtree rooted at 'node'
    // rebalances on the way back up, and returns the (possibly new) root of this subtree
    private Node<T> Insert(Node<T> node, T value)
    {
        // normal BST insertion
        if (node == null)
            return new Node<T>(value); // create the node here

        if (value.CompareTo(node.Value) < 0)
            node.Left = Insert(node.Left, value);
        else if (value.CompareTo(node.Value) > 0)
            node.Right = Insert(node.Right, value);
        else
            return node; // value already exists

        // update this node's height now that a child may have changed
        UpdateHeight(node);

        // check whether this node became unbalanced, and fix it
        int balance = BalanceFactor(node);

        if (balance > 1 && value.CompareTo(node.Left.Value) < 0) // left-left
            return RotateRight(node);

        if (balance < -1 && value.CompareTo(node.Right.Value) > 0) // right-right
            return RotateLeft(node);

        if (balance > 1 && value.CompareTo(node.Left.Value) > 0) // left-right
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        if (balance < -1 && value.CompareTo(node.Right.Value) < 0) // right-left
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        // if already balanced we return this node unchanged
        return node;
    }

    // returns true if 'value' is in the tree, false otherwise
    public bool Search(T value)
    {
        Node<T> current = root; // start at the top

        while (current != null)
        {
            if (value.CompareTo(current.Value) == 0)
                return true; // found it
            else if (value.CompareTo(current.Value) < 0)
                current = current.Left; // target is smaller
            else
                current = current.Right; // target is larger
        }

        return false; // we are at the bottom, so not found
    }

    // returns the node with the smallest value in this subtree (leftmost node)
    private Node<T> MinValueNode(Node<T> node)
    {
        Node<T> current = node;
        while (current.Left != null)
            current = current.Left;
        return current;
    }

    public void Delete(T value)
    {
        root = Delete(root, value);
    }

    // deletes value from the subtree rooted at 'node'
    // rebalances on the way back up, and returns the new root of this subtree
    private Node<T> Delete(Node<T> node, T value)
    {
        // find the node
        if (node == null)
            return null; // value isn't in the tree

        if (value.CompareTo(node.Value) < 0)
            node.Left = Delete(node.Left, value);
        else if (value.CompareTo(node.Value) > 0)
            node.Right = Delete(node.Right, value);
        else
        {
            // found the node to delete
            if (node.Left == null) // leaf or only a right child
                return node.Right;
            else if (node.Right == null) // only a left child
                return node.Left;

            // if there are two children, replace this node's value with its in-order successor
            // then delete the successor from the right subtree
            Node<T> successor = MinValueNode(node.Right);
            node.Value = successor.Value;
            node.Right = Delete(node.Right, successor.Value);
        }

        // update this node's height
        UpdateHeight(node);

        // rebalance if this node became unbalanced
        int balance = BalanceFactor(node);

        if (balance > 1 && BalanceFactor(node.Left) >= 0) // left-left
            return RotateRight(node);

        if (balance > 1 && BalanceFactor(node.Left) < 0) // left-right
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        if (balance < -1 && BalanceFactor(node.Right) <= 0) // right-right
            return RotateLeft(node);

        if (balance < -1 && BalanceFactor(node.Right) > 0) // right-left
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }



    public int GetHeight()
    {
        return Height(root);
    }

    // every value in ascending order
    public List<T> InOrder()
    {
        List<T> result = new List<T>();
        InOrder(root, result);
        return result;
    }

    private void InOrder(Node<T> node, List<T> result)
    {
        if (node == null) return;
        InOrder(node.Left, result);   // smaller values first
        result.Add(node.Value);       // then this node
        InOrder(node.Right, result);  // then larger values
    }

    public bool IsBalanced()
    {
        return IsBalanced(root);
    }

    private bool IsBalanced(Node<T> node)
    {
        if (node == null) return true;
        int bf = BalanceFactor(node);
        if (bf < -1 || bf > 1) return false;
        return IsBalanced(node.Left) && IsBalanced(node.Right);
    }
}