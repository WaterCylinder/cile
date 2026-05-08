using Godot;
using System;

[GlobalClass]
public partial class CardBehavior : Behavior
{
	public Card card;

	public void ResourceAdd()
	{
		int num = (int)Arg("num");
		card.owner.ResourcesPointChange(num);
	}
}
