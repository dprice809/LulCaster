﻿using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.ViewModels
{
  public class PresetControlViewModel : ViewModelBase
  {
    public event EventHandler<PresetViewModel> SelectionChanged;

    #region "Private Members"
    private ObservableCollection<PresetViewModel> _presets = new ObservableCollection<PresetViewModel>();
    private PresetViewModel _selectedPreset;
    private IPresetListController _presetController;
    #endregion "Private Members"

    #region "Properties"
    private void OnPresetSelectionChanged()
    {
      SelectionChanged?.Invoke(this, SelectedPreset);
    }

    public ObservableCollection<PresetViewModel> Presets
    {
      get
      {
        return _presets;
      }
      set
      {
        _presets = value ?? new ObservableCollection<PresetViewModel>();
        OnPropertyChanged(nameof(Presets));
      }
    }

    public PresetViewModel SelectedPreset
    {
      get
      {
        return _selectedPreset;
      }
      set
      {
        _selectedPreset = value;
        OnPropertyChanged(nameof(SelectedPreset));
        OnPresetSelectionChanged();
      }
    }
    #endregion "Properties"

    public PresetControlViewModel(IPresetListController presetController)
    {
      _presetController = presetController;

      Initialize();
    }

    private void Initialize()
    {
      var presetTask = _presetController.GetAllAsync();
      presetTask.Wait();
      Presets = new ObservableCollection<PresetViewModel>(presetTask.Result);
    }

    #region "Control Events"
    public async Task NewItemClickedAsync(ButtonClickArgs args)
    {
      var e = (ButtonClickArgs)args;

      var title = $"{e.Action} {e.ItemDescriptor}";
      var message = $"{e.Action} {e.ItemDescriptor}: ";
      var selectedItem = SelectedPreset;
      var results = CrudDialogProvider.Show(new NestedViewModel<PresetViewModel>(title, message, selectedItem, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var newPreset = await _presetController.CreateAsync(results.InnerResults.Name, results.InnerResults.ProcessName);
      Presets.Add(newPreset);
      SelectedPreset = newPreset;
    }

    public async Task DeleteItemClickedAsync(object sender, ButtonClickArgs e)
    {
      if (SelectedPreset == null
            || MessageBoxProvider.ShowDeleteDialog("Preset")?.DialogResult != DialogResults.Yes)
        return;

      await _presetController.DeleteAsync(SelectedPreset);
      Presets.Remove(SelectedPreset);
      SelectedPreset = null;
    }

    public async Task EditItemClickedAsync(object sender, ButtonClickArgs e)
    {
      var results = CrudDialogProvider.Show(new NestedViewModel<PresetViewModel>("Editing Preset", "Editing Preset: ", SelectedPreset, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var existingPresetIndex = Presets.IndexOf(SelectedPreset);

      SelectedPreset.Name = results.InnerResults.Name;
      SelectedPreset.ProcessName = results.InnerResults.ProcessName;

      await _presetController.UpdateAsync(SelectedPreset);
      Presets[existingPresetIndex] = SelectedPreset;
    }
    #endregion
  }
}
