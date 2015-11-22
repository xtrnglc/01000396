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

namespace Entities
{
    public class Entity
    {
        private string type;
        private string name;
        private string path;
        private Entity parent;
        private int size;
        private string content;
        private Dictionary<string, Entity> children;

        /// <summary>
        /// Constructor for folder, zip file and text file
        /// </summary>
        /// <param name="ArgType"></param>
        /// <param name="fileName"></param>
        /// <param name="ArgParent"></param>
        public Entity(string ArgType, string fileName, Entity ArgParent)
        {
            name = fileName;
            type = ArgType;
            parent = ArgParent;
            ArgParent.children.Add(fileName, this);

            if (type.Equals("text"))
            {
                content = "";
                children = null;
            }
            else
            {
                content = null;
                children = new Dictionary<string, Entity>();
            }
        }

        /// <summary>
        /// Constructor for drive
        /// </summary>
        /// <param name="driveName"></param>
        public Entity(string driveName)
        {
            type = "drive";
            name = driveName;
            path = driveName;
            content = null;
            children = new Dictionary<string, Entity>();
        }

        /// <summary>
        /// Sets content of text file
        /// </summary>
        /// <param name="newContent"></param>
        public void writeToText(string newContent)
        {
            content = newContent;
            size = content.Length;
        }

        /// <summary>
        /// Return the type of the entity
        /// </summary>
        /// <returns></returns>
        public string returnType()
        {
            return type;
        }

        /// <summary>
        /// Returns the path of the entity
        /// </summary>
        /// <returns></returns>
        public string getPath()
        {
            if (type.Equals("drive"))
            {
                return path;
            }
            else
            {
                return parent.getPath() + "\\" + name;
            }
        }

        /// <summary>
        /// Returns the name of the entity
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return name;
        }

        /// <summary>
        /// Helper method to remove child from parent
        /// </summary>
        /// <param name="childName"></param>
        public void removeChild(string childName)
        {
            children.Remove(childName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        public void addChild(Entity child)
        {
            children.Add(child.getName(), child);
        }

        /// <summary>
        /// Returns the size of the entity,
        /// For a text file, it is the length of the contents
        /// For a folder/drive, it is the sum of the sizes of all its entities
        /// For a zip file, it is the sum of the sizes of all its entities divided by two
        /// </summary>
        /// <returns></returns>
        public int getSize()
        {
            int toReturn = 0;
            if (type.Equals("text"))
            {
                toReturn = size;
                size = content.Length;
                return toReturn;
            }
            else if (type.Equals("drive") || type.Equals("folder"))
            {
                foreach(KeyValuePair<string, Entity> c in children)
                {
                    size += c.Value.getSize();
                }
                toReturn = size;
                size = 0;
                return toReturn;
            }
            else
            {
                foreach (KeyValuePair<string, Entity> c in children)
                {
                    size += c.Value.getSize();
                }
                toReturn = size;
                size = 0;
                return toReturn / 2;
            }        
        }
    }
}
