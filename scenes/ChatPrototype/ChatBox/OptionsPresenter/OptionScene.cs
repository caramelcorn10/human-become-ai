using Godot;
using YarnSpinnerGodot;

[GlobalClass]
public partial class OptionScene : Control
{
	[Export] public Button button;

	private DialogueOption _option;
	private OptionsPresenter _presenter;

	public void Setup(DialogueOption option, OptionsPresenter presenter)
	{
		_option = option;
		_presenter = presenter;
		
		button.Text = option.Line.Text.Text;
		
		button.Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		// Tell the presenter this option was chosen
		_presenter.OnOptionSelected(_option);
	}
}
