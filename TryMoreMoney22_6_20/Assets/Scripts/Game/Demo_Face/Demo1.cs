using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Demo_Face
{
    public class Demo1
    {
        /// <summary>
        /// 使用方式
        /// Demo1 demo1 = new Demo1();
        /// DDebug.Log(demo1.UnzipString("f3d2gx2", ""));
        /// 
        /// f3d2gx2  => fffddgxx
        /// 使用递归 ，每次递归都会拆除第一个和第二个数，第一个作为copy字，第二个作为复制值
        /// 如果第二个复制值不是int类型，下一个递归的字符串将从第一个字开始，如果是int类型，就从第二个字符串开始继续递归，直到字符串长度为空或者为1
        /// </summary>
        /// <param name="target"></param>
        public string UnzipString(string target , string combineString = "")
        {
            if (target.Length<=0 || string.IsNullOrEmpty(target))
            {
                Debug.Log("target is null or empty");
                return combineString;
            }
            string uS = target.Substring(0, 1);
            if (target.Length <= 1)
            {
                return combineString + uS;
            }

            string endS = target.Substring(1, 1);
            if (int.TryParse(endS , out var num))
            {
                for (int i = 0; i < num; i++)
                {
                    combineString = combineString + uS;
                }
                return UnzipString(target.Substring(2, target.Length - 2), combineString);
            }
            else
            {
                combineString = combineString + uS;
                return UnzipString(target.Substring(1, target.Length - 1), combineString);
            }
        }
    }

    
    /// <summary>
    /// demo1 结束
    ///
    ///
    /// demo2 开始
    /// </summary>
    
    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode LeftNode { get; set; }
        public TreeNode RightNode { get; set; }

        public TreeNode(int value)
        {
            Value = value;
        }

        public void Print()
        {
            Debug.Log($"print node's value is {Value}");
        }

    }

    public class TreeLink
    {
        public TreeNode RootNode { get; private set; }
        public int LeftTreeLength { get; private set; }
        public int RightTreeLength { get; private set; }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        public void AppendNode(int value)
        {
            if (RootNode == null)
            {
                RootNode = new TreeNode(value);
                return;
            }
            var headNoed = RootNode;

            if (LeftTreeLength == RightTreeLength)
            {
                while (headNoed.LeftNode!=null)
                {
                    headNoed = headNoed.LeftNode;
                }
                headNoed.LeftNode = new TreeNode(value);
                LeftTreeLength++;
            }
            else
            {
                while (headNoed.RightNode!=null)
                {
                    headNoed = headNoed.RightNode;
                }
                headNoed.RightNode = new TreeNode(value);
                RightTreeLength++;
            }
        }


        /// <summary>
        /// 层级遍历 打印值
        /// </summary>
        public void Print()
        {
            if (RootNode == null) {  
                return;  
            }

            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(RootNode);  
            while (queue.Count != 0) 
            {  
                TreeNode node = queue.Dequeue();  
                node.Print();
                if (node.LeftNode != null) {  
                    queue.Enqueue(node.LeftNode);  
                }  
                if (node.RightNode != null) {  
                    queue.Enqueue(node.RightNode);  
                }  
            }  
            
        }
    }

    public class Demo2
    {
        
        /// <summary>
        /// 使用方式
        /// Demo2 demo2 = new Demo2();
        /// demo2.CombineArr(new[] {9, 4, 2, 3,7,6,5} , new []{11,8,7,1});
        ///
        /// 首先使用归并排序对俩组分数排序
        /// 因为需要按照冠亚军位置顺序，在这里我使用了树得层级遍历 ， 除了根节点，其他只有单一节点（不知道有没有理解对）
        /// 将排序完的俩组树 进行树中的存放
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        public void CombineArr(int[] arr1 , int[] arr2)
        {
            arr1 = Sort(arr1 , 0 , arr1.Length - 1);
            arr2 = Sort(arr2 , 0 ,arr2.Length - 1);
            int arr1Index = 0;
            int arr2Index = 0;
            TreeLink treeLink = new TreeLink();
            while (arr1Index < arr1.Length)
            {
                if (arr1[arr1Index] >= arr2[arr2Index] || arr2Index >= arr2.Length)
                {
                    treeLink.AppendNode(arr1[arr1Index]);
                    arr1Index++;
                }
                else
                {
                    treeLink.AppendNode(arr2[arr2Index]); 
                    arr2Index++;
                }
                
            }

            while (arr2Index < arr2.Length)
            {
                treeLink.AppendNode(arr2[arr2Index]); 
                arr2Index++;
            }
            treeLink.Print();
        }
        
        /// <summary>
        /// 归并排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int[] Sort(int[] array, int head, int end)
        {
            if (head >= end) return default;
            int c = (head + end) / 2;
            Sort(array, head, c);
            Sort(array, c + 1, end);
            return Merge(array, head, c, end);
        }
 
        private int[] Merge(int[] array, int head, int c, int end)
        {
            int[] lArr = new int[c - head + 2];
            int[] rArr = new int[end - c + 1];
            lArr[c - head + 1] = 0;
            rArr[end - c] = 0;
 
            for (int i = 0; i < c - head + 1; i++)
            {
                lArr[i] = array[head + i];
            }
 
            for (int i = 0; i < end - c; i++)
            {
                rArr[i] = array[c + 1 + i];
            }
 
            var j = 0;
            var k = 0;
            for (var i = 0; i < end - head + 1; i++)
            {
                if (lArr[j] >= rArr[k])
                {
                    array[head + i] = lArr[j];
                    j++;
                }
                else
                {
                    array[head + i] = rArr[k];
                    k++;
                }
            }

            // string d = "";
            // for (int i = 0; i < array.Length; i++)
            // {
            //     d += array[i] + " ";
            // }
            // Debug.Log($"分次输出数组为·:{d}");
            return array;
         
        }
        
    }
}