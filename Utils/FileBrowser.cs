
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TemporalThievery.Utils
{
    /// <summary>
    /// This class provides a simple keyboard-operated file browser that can be used by multiple classes for selecting files.
    /// It is temporary and should eventually be replaced with a more interactive GUI.
    /// </summary>
    internal class FileBrowser
    {
        /// <summary>
        /// If specified, the file browser will start in this directory and be restricted to it.
        /// </summary>
        internal string homeDir;

        /// <summary>
        /// The directory that the file browser is currently viewing.
        /// </summary>
        internal string currentDir;
        /// <summary>
        /// A list of subdirectories and files matching fileType within the current directory.
        /// </summary>
        internal List<string> currentDirItems;

        /// <summary>
        /// How many of the items in currentDirItems are directories.
        /// </summary>
        internal int subDirectoryCount;

        /// <summary>
        /// The index of the currently selected file.
        /// </summary>
        internal int cursor;
        internal string fileType;

        bool lockToHomeDir;

        internal FileBrowser(string homeDir = "", bool lockToHomeDir = false, string fileType = "")
        {
            this.homeDir = Path.GetFullPath(homeDir);
            currentDir = this.homeDir;
            this.lockToHomeDir = lockToHomeDir;
            this.fileType = fileType;

            currentDirItems = [];
            RefreshList();
        }

        internal bool MoveUpDirectory()
        {
            try
            {
                // Find the parent directory
                string newDir = Directory.GetParent(currentDir).ToString();
                // If locked to home dir, check if we're leaving the home dir before proceeding
                if (lockToHomeDir && !newDir.StartsWith(homeDir))
                {
                    return false;
                }
                currentDir = newDir;
                RefreshList();
                return true;
            }
            // This occurs if the user attempts to move up a directory while at the root directory
            catch (NullReferenceException) 
            {
                return false;
            }
        }

        internal void SetCurrentDir(string newCurrentPath)
        {
            if (!Directory.Exists(newCurrentPath))
            {
                throw new DirectoryNotFoundException(newCurrentPath);
            }
            if (lockToHomeDir && !newCurrentPath.StartsWith(homeDir))
            {
                throw new InvalidOperationException(newCurrentPath +" is not within the home directory.");
            }
            currentDir = newCurrentPath;
            RefreshList();
        }

        internal string GetCurrentDir()
        {
            return currentDir;
        }

        internal void RefreshList()
        {
            cursor = 0;
            currentDirItems.Clear();

            string[] dirs = Directory.GetDirectories(currentDir);
            subDirectoryCount = 0;
            foreach (string dir in dirs)
            {
                subDirectoryCount++;
                currentDirItems.Add(dir);
            }

            string[] files = Directory.GetFiles(currentDir);
            foreach (string file in files)
            {
                if (fileType == "" || Path.GetExtension(file) == fileType)
                {
                    currentDirItems.Add(file);
                }
            }
        }

        internal bool IsSelectedItemDirectory()
        {
            return cursor < subDirectoryCount;
        }

        internal void OpenSelectedDirectory()
        {
            string path = GetSelectedPath();
            if (Directory.Exists(path))
            {
                currentDir = path;
                RefreshList();
            }
            else
            {
                throw new InvalidOperationException("Attempted to open a non-directory path as a directory.");
            }
        }

        internal string OpenSelectedFile()
        {
            string path = GetSelectedPath();
            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                throw new InvalidOperationException("Attempted to open a non-directory path as a directory.");
            }
        }

        internal string GetSelectedPath()
        {
            return currentDirItems[cursor];
        }

        internal void MoveCursorDown()
        {
            cursor = (cursor + 1) % currentDirItems.Count;
        }

        internal void MoveCursorUp()
        {
            cursor--;
            if (cursor < 0)
            {
                cursor = currentDirItems.Count - 1;
            }
        }
    }
}