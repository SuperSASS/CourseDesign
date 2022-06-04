﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CourseDesign.Command.Classes
{
    /// <summary>
    /// 战术人形类
    /// <para>从数据库中导入</para>
    /// </summary>
    /// <list type="number">
    /// <item><c>ID</c> - 人形编号</item>
    /// <item><c>Name</c> - 人形名称</item>
    /// <item><c>Rarity</c> - 人形稀有度</item>
    /// <item><c>Type</c> - 人形种类</item>
    /// <item><c>Artwork</c> - 人形立绘</item>
    /// </list>
    internal class T_DollClass
    {
        public enum typeEnum { hg, smg, rf, ar, mg, sg }; // 计划类型的枚举

        private int id;
        private string name;
        private int rarity;
        private typeEnum type;
        private string artworkPath;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }
        public typeEnum Type
        {
            get { return type; }
            set { type = value; }
        }
        public string ArtworkPath
        {
            get { return artworkPath; }
            set { artworkPath = value; }
        }

        /// <summary>
        /// 战术人形的构造函数
        /// </summary>
        /// <param name="id">人形ID</param>
        /// <param name="name">人形名称</param>
        /// <param name="rarity">人形稀有度</param>
        /// <param name="type">人形种类</param>
        /// <param name="artwork">人形立绘</param>
        public T_DollClass(int id, string name, int rarity, typeEnum type, string artworkPath)
        {
            ID = id;
            Name = name;
            Rarity = rarity;
            Type = type;
            ArtworkPath = artworkPath;
        }
    }
}
