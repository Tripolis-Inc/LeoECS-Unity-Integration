// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TripolisInc.EcsCore.Editor
{
    internal static class EcsEditorStyleUtils
    {
        private const float BorderRadius = 5f;
        private const float BorderWidth = 1.5f;
        private const float PaddingLeft = 5f;
        
        private static readonly StyleColor BorderColor = new StyleColor(new Color(51 / 255f, 51 / 255f, 51 / 255f, 1f));

        public static void SetupComponentStyle(VisualElement vs, Type compType)
        {
            vs.style.backgroundColor = EcsEditorUtils.GetComponentColor(compType);
        
            vs.style.borderBottomColor = BorderColor;
            vs.style.borderLeftColor = BorderColor;
            vs.style.borderRightColor = BorderColor;
            vs.style.borderTopColor = BorderColor;

            vs.style.borderBottomWidth = BorderWidth;
            vs.style.borderLeftWidth = BorderWidth;
            vs.style.borderRightWidth = BorderWidth;
            vs.style.borderTopWidth = BorderWidth;

            vs.style.borderBottomLeftRadius = BorderRadius;
            vs.style.borderBottomRightRadius = BorderRadius;
            vs.style.borderTopLeftRadius = BorderRadius;
            vs.style.borderTopRightRadius = BorderRadius;

            vs.contentContainer.style.paddingRight = PaddingLeft;
        }
    }
}