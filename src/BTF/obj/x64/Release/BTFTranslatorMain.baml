   M S B A M L   `   `   `  ���� >  :BTF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:BTFBTF      BTF.BTFTranslator   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     #localclr-namespace:BTF     -     � Title$   브레인퍽 번역기��$��550.5��$
��1004��
������� � AllowsTransparency .  � WindowStyle$
 None=����	#FF1F1C1C$��1q�$��4q� � Icon$ resource/brainfuck.png�� J� 	AllowDrop . .���  .  � RowDefinitions �� $	��20* ��� $
��117* �.+�]� -   ��. $	��252��$	��Top=�$��2��.+��� -   $	��BTF�����	#FF1F1C1C$	��Top=�$��44��._��� $��	10,4,35,8q�$��16(����	#FFFDFCFC$��Noto Sans CJK KR Medium)����� *Title+ �� RelativeSource �� *FindAncestor+ �� AncestorType$ WindowL� �� FallbackValue$ Title������ *BTF+o� $
��Left=�$	��Top=�$
��1003��$��55.667��$��	0,-14,0,0q����	#FF1F1C1C���	#FF0F0F0F	 J� Effect	 3  -System.Windows.Media.Effects.DropShadowEffect  
   
BlurRadius$
 10Q�   	Direction$	 -90Q�   RenderingBias$ Quality=�   ShadowDepth$ 0Q�.@�m� -   $��menus��$��	   파일��$��55.667��
�������$��68��$��Noto Sans CJK KR Medium)�|�.   J� RenderTransformOrigin$ 0.5243,0.348&� � Cursor$
 Hand��$��14.667(�$
��Bold"�.@�m� -   $��불러오기��$��0q����	#FF040404$��맑은 고딕)�$��Normal"�$��12(�m� -   $��저장��
���   �$��Normal"�$��맑은 고딕)�$��12(�m� -   $��다른이름으로저장�����	#FF0A0A0A$��맑은 고딕)�$��Normal"�$��12(��� m� $��최근사용한파일�����	#FF080808$��맑은 고딕)�$��Normal"�$��12(�.@�.@�Noto Sans CJK KR Medium m� $��
ReadMe.txt��m� $��Schedule.xls��m� -   $��	   종료��$��55.667��
�������$��68��$��14.667(�$��Noto Sans CJK KR Medium)�|�.  $
 Hand��$
��Bold"��� -	    ��Name$ LineNum��$�� ��$	��400��$	��Top=�$��
0,44,961,0q�$��1�����	#FF252121�� -
    ��Name$ comboBox��$	��102��$��21��$��
0,18,381,0q�$	��Top=�$��Right=�$��1�����	#FF080808����  �� EndPoint$	 0,1&� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFF0F0F0�� � Offset$ 0Q�� $ White��$ 1Q�.@��� $��인간언어���� $
��Java���� $
��Ook!���� $	��C++���� $��C#���� $��
Javascript���� $��As3.0���� $��Swift���� -   $��	CodeInput��$
��Left=�$	��474��$��
41,44,0,24q���. $ �� HorizontalScrollBarVisibility$ Hidden=�" �� VerticalScrollBarVisibility$
 Auto=�$��0q� �� AcceptsReturn . ���	#FF1F1C1C���	#FF0B0B0C$��1��$��	Mono sans)�
�������$��13.333(� � ContextMenu �� .@�m� $��잘라내기�� m� Command$ ApplicationCommands.Cut��m� $��복사��$ ApplicationCommands.Copy��m� -   $��붙여넣기�� � 	Resources  �� 
TargetType �� Property �� Value��%3   'J�       �� " J�.���  �@��Margin$ 0q�. �/� .T�J� �� -   $ label1��$��
Brain Fuck��$��Right=�$��0,0,899,430q�   SimpleLabel#���   $��Bottom=�$��84��$��14.667(����	#FF080707$��1���� -    ��Name$ 
CodeOutput��$
��Left=�$	��487��$��515,44,0,24q���. $
 Auto=�$
 Auto=����	#FF2B2828$��0q� . $��1q�  �� 
IsReadOnly  . ���	#FF0B0B0C$��1��$��	Mono sans)�
�������$��13.333(�. �/� .T�J� �� -   $��Lineinfo��$!��줄:47   열:4   문자:500��
����������	#FF0B0B0C���	#FF252121$��
0,444,0,-2q�$��맑은 고딕)�$��1��$��Right=�$
��1002���� -   ! ��Name$! 번역��$��번역��$
��Left=�$��
432,19,0,0q�$��1��$	��Top=�$��75��
���   �
�������$��1q�
���   ��� -   $! 번역_Copy��$��복사��$
��Left=�$	��Top=�$��67��$��
925,18,0,0q�$��1��
���   �
��������� -   $ LineNumLabel�����	#FF252121$��Center=�$��16,0,961,24q�$��Bottom=�$��25��$��13.333(����	#FFF17648$��1��$��	Mono Sans)�$	��400��$��Center=�  . $��0q�$ Hidden=� ��%3   'J�       �� " J�.���  �@��Margin$ 0q�. �/� .T�J� �� -   $! 	EXEoutput��$��.exe로 내보내기��$
��Left=�$��
325,19,0,0q�$��1��$	��Top=�$��95��
���   �
�������$��1q�
���   �