using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using YarnSpinnerGodot;

[GlobalClass]
public partial class OptionsPresenter : Control, DialoguePresenterBase
{
	[Export] public PackedScene optionScene;
	[Export] public HBoxContainer hboxContainer;
	
	public List<IActionMarkupHandler> ActionMarkupHandlers { get; } = [];
	
	private YarnTaskCompletionSource<DialogueOption?> _currentSelectionSource;

	public async YarnTask<DialogueOption?> RunOptionsAsync(DialogueOption[] dialogueOptions,
		CancellationToken cancellationToken)
	{
		_currentSelectionSource = new YarnTaskCompletionSource<DialogueOption?>();
		foreach (var option in dialogueOptions)
		{
			if (!option.IsAvailable) continue; 

			var instance = optionScene.Instantiate();
			hboxContainer.AddChild(instance);
			
			if (instance is OptionScene optionInstance)
			{
				optionInstance.Setup(option, this);
			}
		}
		
		var selectedOption = await _currentSelectionSource.Task;
		
		foreach (var child in hboxContainer.GetChildren())
		{
			child.QueueFree();
		}

		return selectedOption;
	}
	public void OnOptionSelected(DialogueOption option)
	{
		_currentSelectionSource?.TrySetResult(option);
	}
	
	// Default implementation for other interface methods
	public YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token) => YarnTask.CompletedTask;
	public YarnTask OnDialogueStartedAsync() => YarnTask.CompletedTask;
	public YarnTask OnDialogueCompleteAsync() => YarnTask.CompletedTask;
}
