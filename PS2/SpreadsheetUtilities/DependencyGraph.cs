using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// s1 depends on t1 --> t1 must be evaluated before s1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Uses an array of linked lists to manage dependencies.
        /// For example (a,b) (a,c) (b,d) (d)
        /// The dependents array will be
        /// 
        /// a -> b -> c
        /// b -> d
        /// c -> null
        /// d -> d
        /// 
        /// dependents(a) will traverse the linked list that begins with "a" and return all values that are in the linked list. In this case "b" and "d"
        /// 
        /// Another array of linked lists will also manage dependees
        /// The dependees array will be
        /// 
        /// a -> null
        /// b -> a
        /// c -> a
        /// d -> b -> d
        ///
        /// </summary>
        private LinkedList<string>[] DependentsList;
        private LinkedList<string>[] DependeesList;
        private int count;
        private int size;
        private int current;


        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            DependentsList[0] = null;
            DependeesList[0] = null;
            count = 0;
            size = 0;
            current = -1;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get { return 0; }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            return null;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            return null;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   s depends on t
        ///
        /// </summary>
        /// <param name="s"> s cannot be evaluated until t is</param>
        /// <param name="t"> t must be evaluated first.  S depends on T</param>
        public void AddDependency(string s, string t)
        {
            int DependentFlag = 0;                                                                  //Dependent Flag to keep track of whether a new dependent list is added, 0 indicates not added, 1 indicates added
            int DependeeFlag = 0;                                                                   //Dependee Flag to keep track of whether a dependee list exists or not

            for(int i = 0; i < count; i++)
            {
                if (DependentsList[i].First.Equals(s) && DependentFlag == 0)                        //Check to see if s already exists in the linked list, if it does then append a new depedent
                {
                    if (!DependentsList[i].Contains(t))                                             //Check to see if that dependent already exists, if it does then do nothing, else append a new dependent
                    {
                        DependentsList[i].AddLast(t);
                        DependentFlag = 1;
                        size++;
                    }
                }   
            }

            if(DependentFlag == 0)
            {
                current++;
                size++;
                DependentsList[current].AddFirst(s);
                DependentsList[current].AddLast(t);
                DependeesList[current].AddFirst(s);

                for(int i = 0; i < count; i++)
                {
                    if (DependentsList[i].First.Equals(t)){
                        DependeeFlag = 1;
                    }
                }

                if(DependeeFlag == 0)
                {
                    DependentsList[current + 1].AddFirst(t);
                    DependeesList[current + 1].AddFirst(t);
                }

            }





        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
        }

    }




}


