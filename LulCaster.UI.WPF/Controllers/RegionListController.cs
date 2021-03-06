﻿using LulCaster.UI.WPF.Config.UserSettings;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Controllers
{
  public class RegionListController : IRegionListController
  {
    private readonly IRegionConfigService _regionConfigService;

    public RegionListController(IRegionConfigService configService)
    {
      _regionConfigService = configService;
    }

    public RegionViewModel Create(Guid presetId, string regionName)
    {
      return _regionConfigService.Create(presetId, regionName);
    }

    public void WriteAll(string filePath, IEnumerable<RegionViewModel> regions)
    {
      _regionConfigService.WriteAll(filePath, regions);
    }

    public IEnumerable<RegionViewModel> GetAll(string importFilePath)
    {
      return _regionConfigService.GetAllRegions(importFilePath);
    }

    public IEnumerable<RegionViewModel> GetRegions(Guid presetId)
    {
      return _regionConfigService.GetAllRegionsAsViewModels(presetId);
    }

    public void Update(Guid presetId, RegionViewModel region)
    {
      _regionConfigService.Update(presetId, region);
    }

    public async Task UpdatAsync(Guid presetId, RegionViewModel region)
    {
      await _regionConfigService.UpdateAsync(presetId, region);
    }

    public void Delete(Guid presetId, Guid regionId)
    {
      _regionConfigService.Delete(presetId, regionId);
    }
  }
}