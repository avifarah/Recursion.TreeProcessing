#nullable enable
using System;

namespace TreeProcessing
{
    class Program
    {
        static void Main(string[] _)
        {
            // Generate the BST tree.  For which BstInsert() needs to be completed.
            var bst = Tree.Insert6Node();
            
            // Use ToString() to write the tree such that each subtree is (node subSubTree subSubTree)
            Console.WriteLine("6 node tree");
            Console.WriteLine(bst.ToString());

            // Write the mirror of the tree
            Console.WriteLine();
            Console.WriteLine("Mirror of original tree");
            var mirror = bst.Mirror();
            Console.WriteLine(mirror.ToString());

            // Make sure that mirror is now identical to bst (we flipped the subTrees in place
            Console.WriteLine();
            Console.WriteLine("IsSameTree mirror and bst");
            var isSame = Tree.IsSameTree(bst, mirror);
            Console.WriteLine($"Tree.IsSameTree(bst, mirror) = {isSame}.  Expected result: True");

            // Mirror the mirror such that we leave the original mirror untouched
            Console.WriteLine();
            Console.WriteLine("Mirror the mirrored tree.  Generate a new tree");
            var orig = mirror.MirrorNewTree();
            Console.WriteLine(orig.ToString());
            isSame = Tree.IsSameTree(bst, orig);
            Console.WriteLine($"Tree.IsSameTree(bst, orig) = {isSame}.  Expected result: false");

            // Double Tree
            Console.WriteLine();
            Console.WriteLine("Double each node");
            orig.DoubleTree();
            Console.WriteLine(orig.ToString());

            // Is Binary Search Tree
            Console.WriteLine();
            Console.WriteLine("Is Binary Search Tree");
            var isBst = bst.IsBinarySearchTree();
            Console.WriteLine($"bst.IsBinarySearchTree():    {isBst,5}.  Expected: false");
            isBst = mirror.IsBinarySearchTree();
            Console.WriteLine($"mirror.IsBinarySearchTree(): {isBst,5}.  Expected: false");
            isBst = orig.IsBinarySearchTree();
            Console.WriteLine($"orig.IsBinarySearchTree():   {isBst,5}.  Expected: true");
        }
    }

    public class Tree
    {
        public Tree? Left { get; set; }
        public Tree? Right { get; set; }
        public int Node { get; private set; }

        public Tree(int node)
        {
            Node = node;
            Left = null;
            Right = null;
        }

        /*
        * Insert nodes into the Tree such that if the new node's value <= current 
        * then insert new Node into LEFT subtree Else it belongs to the Right subtree
        *
        *	First node's data, say 100, makes the root.
        *	Second node's data, say 248, therefore it belongs to root's right subtree.
        *          100
        *             \
        *             248
        *
        *	3rd node's data, say 76, makes the left subtree.  Tree now is:
        *          100
        *         /   \
        *       76     248
        *
        *	4th node's data, say 74, is < root, therefore it belongs to the left subtree
        *	it is less than 76 therefore, it belongs to the left of the sub-subTree.
        *  Tree looks like:
        *          100
        *         /   \
        *       76     248
        *      /
        *    74
        *
        * Next node is 178
        * Greater than 100 -- Right subTree
        * Less than 248 -- Left subTree
        * Tree looks like:
        *          100
        *         /   \
        *       76     248
        *      /      /
        *    74     178
        *
        * Next node is 278, which is > 100 and > than 248, Tree looks now like:
        * Tree looks like:
        *          100
        *         /   \
        *       76    248
        *      /     /   \
        *    74    178   278
        *
        * Write a routine that implements the above explanation
        */
        /// <summary>
        /// Syntax:
        /// 		void g(int x) => f(x);
        /// is a short notation for:
        /// 		void g(int x) { f(x); }
        /// 
        /// if on the other hand g(x) as well as f(x) are expected to return a value
        /// say:
        /// 		int g(int x) => f(x)
        /// is a short notation for:
        /// 		int g(int x) { return f(x); }
        /// </summary>
        public void BstInsert(int node) => BstInsert(this, node);

        /// <summary>
        /// Syntax: the ? suffix to the Tree class, in the first parameter input:
        /// 	BstInsert(Tree? subTree, int node)
        /// 			  ^^^^^
        /// Means that the subTree parameter may = null.  Without the ? suffix the
        /// compiler, static analysis, will warn when it thinks that subTree may
        /// be at risk of accepting null.
        /// </summary>
        private static Tree BstInsert(Tree? subTree, int node)
        {
            if (subTree == null)
                return new Tree(node);

            if (node <= subTree.Node)
                subTree.Left = BstInsert(subTree.Left, node);
            else
                subTree.Right = BstInsert(subTree.Right, node);

            return subTree;
        }

        /*
        * Write a ToString() that will return a string containing the tree structure in paretheses.
        * For example tree:
        *       2
        *      / \
        *     1   3
        * Will be presented as: (2 (1 E E) (3 E E))
        */
        public override string ToString()
        {
            var s = ToString(this);
            if (s == null) throw new Exception("A programming bug in static ToString(..) routine");
            return s;
        }

        private static string? ToString(Tree? subTree)
        {
            // Terminating condition
            if (subTree == null)
                return null;

            // Recursive definition
            return $"({subTree.Node} {ToString(subTree.Left)} {ToString(subTree.Right)})";
        }

        /*
        * Write a routine that takes a tree and returns the tree's mirror image.
        * Whatever was left subtree becomes right subtree and visa versa.
        *
        * It is easier to flip right/left in place as opposed to returning a brand new tree.
        */
        public Tree Mirror()
        {
            var m = Mirror(this);
            if (m == null) throw new Exception("Bug in static Mirror(..) method");
            return m;
        }

        // L R n
        private static Tree? Mirror(Tree? subTree)
        {
            // Terminating condition
            if (subTree == null)
                return null;

            // Recursive definition
            _ = Mirror(subTree.Left);
            _ = Mirror(subTree.Right);
            FlipNodesLegs(subTree);
            
            return subTree;
        }
        
        private static void FlipNodesLegs(Tree node)
        {
            var left = node.Left;
            node.Left = node.Right;
            node.Right= left;
        }

        /*
        * Return a new tree that is a mirror of the old tree
        */
        public Tree MirrorNewTree()
        {
            var root = new Tree(Node);
            return MirrorNewTree(this, root);
        }

        private static Tree MirrorNewTree(Tree from, Tree to)
        {
            // Terminating condition 1
            // From == null
            if (from == null)
                return to;

            // Terminating condition 2
            // Left subtree == null, in which case do not duplicate it
            if (from.Left != null)
            {
                var node = new Tree(from.Left.Node);
                to.Right = node;
                _ = MirrorNewTree(from.Left, to.Right);
            }

            // Terminating condition 3
            // Right subtree == null, in which case do not duplicate it
            if (from.Right != null)
            {
                var node = new Tree(from.Right.Node);
                to.Left = node;
                _ = MirrorNewTree(from.Right, to.Left);
            }

            return to;
        }

        /*
        * Compares a given tree to another tree to
        * see if they are structurally identical.
        *
        * n L R
        */
        public static bool IsSameTree(Tree? lhs, Tree? rhs)
        {
            if (lhs == null && rhs == null) return true;
            if ((lhs == null) ^ (rhs == null)) return false;

            // Syntax: the ! suffix to the lhs and rhs variables:
            //		if (lhs!.Node != rhs!.Node) return false;
            //		    ^^^^         ^^^^
            // is a hint to the compiler that it should not generate a warning
            // for lhs and rhs that they can be null for which lhs.Node and 
            // rhs.Node may generate an exception.
            if (lhs!.Node != rhs!.Node) return false;

            if (!IsSameTree(lhs.Left, rhs.Left)) return false;
            return IsSameTree(lhs.Right, rhs.Right);		
        }

        /*
        * Changes the tree by inserting a duplicate node
        * on each nodes's left branch.
        * 
        * So the tree...
        *    2
        *   / \
        *  1   3
        *
        * Is changed to...
        *       2
        *      / \
        *     2   3
        *    /   /
        *   1   3
        *  /
        * 1
        *
        * Uses a recursive helper to recur over the tree
        * and insert the duplicates.
        */
        /// <summary>
        /// We will process L R n
        /// </summary>
        public void DoubleTree() => DoubleTree(this);

        /// <summary>
        /// DoubleTree will turn the original tree into:
        ///               100
        ///              /    \
        ///            100     248
        ///            /       / \
        ///           76     248  \
        ///          /       /     \
        ///         76     178      \
        ///        /       /         \
        ///       74     178          \
        ///      /                   278
        ///     74                   /
        ///                        278
        /// </summary>
        private static void DoubleTree(Tree? tree)
        {
            if (tree == null) return;
            DoubleTree(tree.Left);
            DoubleTree(tree.Right);
            DuplicateNode(tree);
        }

        private static void DuplicateNode(Tree tree)
        {
            var node = new Tree(tree.Node) { Left = tree.Left };
            tree.Left = node;
        }

        /// <summary>
        /// Make sure that each node is greater than or equal 
        /// to all left subnodes and less than all right subnodes
        /// 
        /// Assumption: no tree node is less than int.MinValue or greater than int.MaxValue
        /// </summary>
        //public bool IsBinarySearchTree() => IsBinarySearchTree(this, int.MinValue, int.MaxValue);
        public bool IsBinarySearchTree() => IsBstInefficient(this);

        /// <summary>
        /// Consider the following SubTree
        ///          Middle of tree, for which we have min and max
        ///                     Node: n (min, max)
        ///                    /                 \
        ///           NodeL: nL                   NodeR: nR
        ///           nL > min                    nR > min
        ///           nl <= max                   nR <= max
        ///           new (min, max) = (min, n)   new (min, max) = (n, max)
        /// 
        /// The only node that does not fit well into our model is the root.  The
        /// root's (min, max) is either not checked (root value can be any value)
        /// or if we know the limits of all values then we can check the root 
        /// against those limits and we need not have a special case.
        /// 
        /// We will employ n L R
        /// </summary>
        private static bool IsBinarySearchTree(Tree? subTree, int min, int max)
        {
            // Process node
            if (subTree == null) return true;
            if (!(subTree.Node <= max)) return false;
            if (!(subTree.Node > min)) return false;

            // Process Left SubTree
            var rc = IsBinarySearchTree(subTree.Left, min, subTree.Node);
            if (!rc) return false;

            // Process Right SubTree
            return IsBinarySearchTree(subTree.Right, subTree.Node, max);
        }

        /// <summary>
        /// Process Node.
        /// All nodes in the left subTree are <= subTree.Node and
        /// .. all the notes in the right subTree are > subTree.Node
        /// Then process left subTree
        /// Then process right subTree
        /// </summary>
        private static bool IsBstInefficient(Tree? subTree)
        {
            // Process Node
            if (subTree == null) return true;
            if (!TraverseSubTree(subTree.Node, subTree.Left, (x, y) => x <= y)) return false;
            if (!TraverseSubTree(subTree.Node, subTree.Right, (x, y) => x > y)) return false;

            // Process left SubTree
            if (!IsBstInefficient(subTree.Left)) return false;

            // Process right subTree
            return IsBstInefficient(subTree.Right);
        }

        /// <summary>
        /// Process n L R
        /// </summary>
        private static bool TraverseSubTree(int value, Tree? node, Func<int, int, bool> op)
        {
            // Node
            if (node == null) return true;
            if (!op(node.Node, value)) return false;

            // Left subTree
            var rc = TraverseSubTree(value, node.Left, op);
            if (!rc) return false;

            // Right subTree
            return TraverseSubTree(value, node.Right, op);
        }

        /// <summary>
        ///               100
        ///              /    \
        ///             /      248
        ///            76      / \
        ///           /       /   \
        ///          74      /     \
        ///                178      \
        ///                          278
        /// </summary>
        public static Tree Insert6Node()
        {
            var bst = new Tree(100);
            bst.BstInsert(248);
            bst.BstInsert(76);
            bst.BstInsert(74);
            bst.BstInsert(178);
            bst.BstInsert(278);
            return bst;
        }
    }
}
