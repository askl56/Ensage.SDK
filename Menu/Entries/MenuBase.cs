﻿// <copyright file="MenuBase.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Ensage.SDK.Menu
{
    using System.Reflection;

    using Ensage.SDK.Renderer;

    using PlaySharp.Toolkit.Helper.Annotations;

    using SharpDX;

    public abstract class MenuBase
    {
        protected MenuBase(string name, IView view, IRenderer renderer, object instance, [CanBeNull] PropertyInfo propertyInfo)
        {
            this.Name = name;
            this.View = view;
            this.Renderer = renderer;
            this.DataContext = instance;
            this.PropertyInfo = propertyInfo;
        }

        public object DataContext { get; }

        public bool IsHovered { get; internal set; }

        public string Name { get; }

        public Vector2 Position { get; set; }

        [CanBeNull]
        public PropertyInfo PropertyInfo { get; }

        public IRenderer Renderer { get; }

        /// <summary>
        ///     Gets or sets the size at which this item is rendered. The width will be aligned to the other menu items of the
        ///     parent.
        /// </summary>
        public Vector2 RenderSize { get; set; }

        /// <summary>
        ///     Gets the actual size of the menu item.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return this.View.GetSize(this);
            }
        }

        public IView View { get; }

        public abstract void Draw();

        public bool IsInside(Vector2 screenPosition)
        {
            return this.Position.X <= screenPosition.X
                   && this.Position.Y <= screenPosition.Y
                   && screenPosition.X <= (this.Position.X + this.RenderSize.X)
                   && screenPosition.Y <= (this.Position.Y + this.RenderSize.Y);
        }

        public abstract void OnClick(Vector2 clickPosition);

        public override string ToString()
        {
            return this.Name;
        }
    }
}