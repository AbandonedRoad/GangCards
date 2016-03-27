using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Singleton
{
    public class ItemSingleton
    {
        private static ItemSingleton _instance;

        public List<IItem> OwnedItems { get; private set; }
        public List<IItem> Items { get; private set; }

        /// <summary>
        /// Gets instance
        /// </summary>
        public static ItemSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemSingleton();
                    _instance.Init();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            Items = ResourceSingleton.Instance.GetItems();
            OwnedItems = ResourceSingleton.Instance.GetItems();

            foreach (var item in Items)
            {

            }
        }
    }
}
