/*
Trung Le
11/20/2015
For Proofpoint
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Text.RegularExpressions;

namespace FileSystemNameSpace
{
    public class FileSystem
    {
        private Dictionary<string, Entity> listOfPaths;
        private int count;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileSystem()
        {
            listOfPaths = new Dictionary<string, Entity>();
            count = 0;
        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="pathOfParent"></param>
        public void Create(string type, string name, string pathOfParent)
        {
            if (type.Equals("drive"))
            {
                if(pathOfParent != "")
                {
                    throw new Exception("Drive cannot be contained in another entity");
                }

                if (!listOfPaths.ContainsKey(name))
                {
                    Entity newDrive = new Entity(name);
                    listOfPaths.Add(newDrive.getPath(), newDrive);
                    count++;
                }
                else
                {
                    throw new Exception("Drive with the same name already exists");
                }
                
            }
            else
            {            
                Entity parent;
                if (listOfPaths.TryGetValue(pathOfParent, out parent))
                {
                    if (parent.returnType().Equals("text"))
                    {
                        throw new Exception("Parent cannot be a text file");
                    }
                    Entity newEntity = new Entity(type, name, parent);
                    try
                    {
                        listOfPaths.Add(newEntity.getPath(), newEntity);
                    }
                    catch(Exception e)
                    {
                        throw new Exception("An entity with the same name has already been added");
                    }
                    count++;
                }
                else
                {
                    throw new Exception("Path to parent does not exist");
                }   
            } 
        }

        /// <summary>
        /// Deletes a entity based on its path
        /// </summary>
        /// <param name="path"></param>
        public void Delete(string path)
        {
            Entity e;
            StringBuilder pathToParent = new StringBuilder();
            count = listOfPaths.Count;
            string[] substringsToMatch = Regex.Split(path, "\\\\");
            int length = path.Length;
            if (listOfPaths.TryGetValue(path, out e))
            {
                foreach(KeyValuePair<string, Entity> pair in listOfPaths.ToList())
                {
                    string[] substrings = Regex.Split(pair.Key, "\\\\");
                    
                    /*
                    Each substring and substringToMatch element contains a parent
                    The idea is to step through both of arrays at the same time and match them up with each other, 
                    If the substringsToMatch matches completely with substrings then we can remove the path which corresponds to substrings from the dictionary
                    If there are still things to match in substringsToMatch then we need not remove

                    e.g 
                    substringToMatch = cdrive\fold
                    substrings = cdrive\fold\text1
                    we remove the keyvalue pair that corresponds with substrings

                    substringToMatch = cdrive\fold
                    substrings = cdrive\folder1\text1
                    we do not remove

                    substringsToMatch = cdrive\folder1\folder2
                    substrings = cdrive\folder1
                    we do not remove
                    */



                    if (pair.Key.Contains(path))
                    {
                        for(int i = 0; i < substrings.Count() - 1; i++)
                        {
                            //Deals with drive
                            if (i == 0)
                            {
                                pathToParent.Append(substrings[i]);
                            }
                            //Rest of path
                            else
                            {
                                pathToParent.Append("\\" + substrings[i]);
                            }
                            
                        }
                        //Remove the child from the parent and remove the path from the list of paths
                        listOfPaths.TryGetValue(pathToParent.ToString(), out e);
                        e.removeChild(substrings.Last());
                        listOfPaths.Remove(pair.Key);
                        count--;
                    }
                }
            }
            else
            {
                throw new Exception("Path does not exist");
            }
        }

        //Show a sketch of implementation of the Move operation
        public void Move(string sourcePath, string destinationPath)
        {
            Entity e, parent;
            if (listOfPaths.TryGetValue(destinationPath, out e))
            {
                if (e.returnType().Equals("text"))
                {
                    throw new Exception("Destination path cannot be a text file");
                }
            }

            if (listOfPaths.TryGetValue(sourcePath, out e))
            {
                if (e.returnType().Equals("drive"))
                {
                    throw new Exception("Cannot move a drive");
                }
                /*
                Idea here is to first make a copy of the entity, we store it in local variable e.
                Then we call the delete method and remove it from the system.
                We then create a new entry in the dictionary with the destination path as key and the entity as value
                Finally we add the entity as a child of the parent and increment
                */
                Delete(sourcePath);
                try
                {
                    listOfPaths.Add(destinationPath + "\\" + e.getName(), e);
                    listOfPaths.TryGetValue(destinationPath, out parent);
                    parent.addChild(e);
                    count++;
                }
                catch(Exception excep)
                {
                    throw new Exception("Path already exists in destination folder");
                }
                
            }
            else
            {
                throw new Exception("Path does not exist in source");
            }
        }     

        /// <summary>
        /// Write the contents of a text file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public void WriteToFile(string path, string content)
        {
            Entity e;
            if (listOfPaths.TryGetValue(path, out e))
            {
                string type = e.returnType();
                if (!type.Equals("text"))
                {
                    throw new Exception("Cannot write to a file that is not a text file");
                }
                e.writeToText(content);  
            }
            else
            {
                throw new Exception("Path does not exist");
            }
        }

        /// <summary>
        /// Helper method to print sizes
        /// </summary>
        /// <param name="path"></param>
        public void printSize(string path)
        {
            Entity e;
            if (listOfPaths.TryGetValue(path, out e))
            {
                Console.WriteLine(e.getSize());
            }
            else
            {
                Console.WriteLine("Path does not exist");
            }
        }
    }
}
