
using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupDevPanelBinder : PopupBinder<PopupDevPanelViewModel>
{
    [SerializeField] private Button _creaturesPageButton;
    [SerializeField] private Button _inventoriesPageButton;

    [SerializeField] private DPCreaturesBinder _creaturesPage;
    [SerializeField] private DPInventoriesBinder _inventoriesPage;

    protected override void OnBind(PopupDevPanelViewModel viewModel)
    {
        ViewModel = viewModel;

        _creaturesPage.Bind(ViewModel.CreaturesPageViewModel);
        _inventoriesPage.Bind(ViewModel.InventoriesPageViewModel);
    }

    private void Awake()
    {
        DisalbeAllPages();

        _creaturesPageButton.onClick.AddListener(OpenCreaturesPage);
        _inventoriesPageButton.onClick.AddListener(OpenInventoriesPage);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _creaturesPageButton.onClick.RemoveAllListeners();
        _inventoriesPageButton.onClick.RemoveAllListeners();
    }

    private void OpenInventoriesPage()
    {
        OpenPage(_inventoriesPage.gameObject);
    }

    private void OpenCreaturesPage()
    {
        OpenPage(_creaturesPage.gameObject);
    }

    private void OpenPage(GameObject page)
    {
        DisalbeAllPages();
        page.SetActive(true);
    }

    private void DisalbeAllPages()
    {
        _creaturesPage.gameObject.SetActive(false);
        _inventoriesPage.gameObject.SetActive(false);
    }
}