using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureImportExport.Services
{
    public static class FormControlsService
    {
        public static void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (RuntimeSettingsService.HasUnsavedChanges != true)
            {
                // Set HasUnsavedChanges to true
                RuntimeSettingsService.HasUnsavedChanges = true;

                // Optionally log or debug
                Console.WriteLine("Entry text changed. HasUnsavedChanges set to true.");
            }
        }

        public static void OnPickerSelectedIndexChanged(object? sender, EventArgs e)
        {
            if (RuntimeSettingsService.HasUnsavedChanges != true)
            {
                // Set HasUnsavedChanges to true
                RuntimeSettingsService.HasUnsavedChanges = true;
                // Optionally log or debug
                Console.WriteLine("Picker selected index changed. HasUnsavedChanges set to true.");
            }
        }

        public static void OnSwitchToggled(object? sender, ToggledEventArgs e)
        {
            if (RuntimeSettingsService.HasUnsavedChanges != true)
            {
                // Set HasUnsavedChanges to true
                RuntimeSettingsService.HasUnsavedChanges = true;
                // Optionally log or debug
                Console.WriteLine("Switch toggled. HasUnsavedChanges set to true.");
            }
        }

        public static void OnCheckBoxCheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
            if (RuntimeSettingsService.HasUnsavedChanges != true)
            {
                // Set HasUnsavedChanges to true
                RuntimeSettingsService.HasUnsavedChanges = true;
                // Optionally log or debug
                Console.WriteLine("CheckBox checked changed. HasUnsavedChanges set to true.");
            }
        }

        public static void OnSliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
            if (RuntimeSettingsService.HasUnsavedChanges != true)
            {
                // Set HasUnsavedChanges to true
                RuntimeSettingsService.HasUnsavedChanges = true;
                // Optionally log or debug
                Console.WriteLine("Slider value changed. HasUnsavedChanges set to true.");
            }
        }

        public static void OnEditorTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (RuntimeSettingsService.HasUnsavedChanges != true)
            {
                // Set HasUnsavedChanges to true
                RuntimeSettingsService.HasUnsavedChanges = true;
                // Optionally log or debug
                Console.WriteLine("Editor text changed. HasUnsavedChanges set to true.");
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(VisualElement element) where T : VisualElement
        {
            if (element is T typedElement)
            {
                yield return typedElement;
            }

            if (element is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    foreach (var childOfChild in FindVisualChildren<T>((VisualElement)child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
