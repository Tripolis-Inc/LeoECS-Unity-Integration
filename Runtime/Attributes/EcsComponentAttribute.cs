// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;

namespace TripolisInc.EcsCore.Attributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class EcsComponentAttribute : Attribute
    {
        public string displayedName;
        public string path;

        public EcsComponentAttribute(string displayedName, string path = "")
        {
            this.displayedName = displayedName;
            this.path = string.IsNullOrEmpty(path) ? displayedName : path;
        } 
    }
}