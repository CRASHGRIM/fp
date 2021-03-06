﻿using System.Windows.Forms;
using TagsCloudForm.CloudPainters;
using TagsCloudForm.Common;
using TagsCloudForm.UiActions;

namespace TagsCloudForm.Actions
{
    internal class CircularCloudLayouterWithWordsAction : IUiAction
    {
        private readonly IPainterFactory factory;

        public CircularCloudLayouterWithWordsAction(IPainterFactory factory)
        {
            this.factory = factory;
        }
        public string Category => "CircularCloud";
        public string Name => "LayouterWithWords";
        public string Description => "Создание облака";

        public void Perform()
        {
            factory.Create().GetWithOnError(err => MessageBox.Show(err)).Paint();
        }
    }
}
