   M S B A M L   `   `   `  ����>  :BTF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:BTFBTF  5   	       BTF.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:BTF     5   	   -   5          � Title$   브레인퍽 번역기��5   
   $��550.5��6   $
��1004��6+   
������� � AllowsTransparency .  � WindowStyle$
 None=�6f   ���	#FF1F1C1C$��1q�6�   $��4q�6�    � Icon$ resource/brainfuck.png��6�   .��5	      �  � RowDefinitions 5
   
   �� 5      $	��20* �6   �� 5      $
��117* �6   .+�5   
   ]� -   5        ��. $	��252��5   J   $	��Top=�6W   $��2��6o   .+�5      �� -   5        $	��BTF��5   M   ���	#FF1F1C1C$	��Top=�6,   $��44��6A   ._�5      �� $��	10,4,35,8q�5      $��16(�6+   ���	#FFFDFCFC$��Noto Sans CJK KR Medium)�6P   ��5      �� *Title+ �� RelativeSource �� *FindAncestor+ �� AncestorType$ WindowL� �� FallbackValue$ Title����5   u   �� *BTF+5      o� 5      $
��Left=�6   $	��Top=�6/   $
��1003��6G   $��55.667��6T   $��	0,-14,0,0q�6d   ���	#FF1F1C1C���	#FF0F0F0F J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      	   
BlurRadius$	 10Q�6'   
   	Direction$	
 -90Q�67      RenderingBias$ Quality=�6G      ShadowDepth$ 0Q�6_   .@�5      m� -   5        $��menus��5   �   $��	   파일��6   $��55.667��6*   
�������$��68��6M   $��Noto Sans CJK KR Medium)�6X   |�.   J� RenderTransformOrigin$ 0.5243,0.348&�6�    � Cursor$
 Hand��6�   $��14.667(�6�   $
��Bold"�6�   .@�5      m� -   5        $��불러오기��5      $��0q�6-   ���	#FF040404$��맑은 고딕)�6X   $��Normal"�6k   $��12(�6   6�   m� 5      $��저장��6   
���   �$��Normal"�6?   $��맑은 고딕)�6S   $��12(�6f   m� 5      $��다른이름으로저장��6   ���	#FF0A0A0A$��맑은 고딕)�6I   $��Normal"�6\   $��12(�6p   �� 5      m� 5      $��최근사용한파일��6   ���	#FF080808$��맑은 고딕)�6G   $��Normal"�6Z   $��12(�6n   .@�6|   .@�5      Noto Sans CJK KR Medium m� $��
ReadMe.txt��6#   m� 5      $��Schedule.xls��6#   5      5       m� 5!      -   5        $��	   종료��5!      $��55.667��6*   
�������$��68��6M   $��14.667(�6j   $��Noto Sans CJK KR Medium)�6|   |�.  $
 Hand��6�   $
��Bold"�6�   5#      5$      �� 5%   
   -   5         ��Name$ LineNum��5%      $�� ��6!   $	��400��6,   $	��Top=�69   $��
0,44,961,0q�6Q   $��1��6e   ���	#FF2521216r   �� 5&   
   -   5         ��Name$ comboBox��5&      $	��102��6%   $��21��61   $��
0,18,381,0q�6=   $	��Top=�6Q   $��Right=�6i   $��1��6�   ���	#FF080808��5'      �� 5(       �� EndPoint$	 0,1&�6&    �� 
StartPoint$	 0,0&�65   .��5)      �  � Color$ 	#FFF0F0F0��6#    � Offset$ 0Q�65   � 5*      $ White��6#   $ 1Q�61   5+      .@�5-      �� $��인간언어��6   �� 5.      $��Assembly��6   �� 5/      $
��Ook!��6   �� 50      $	��C++��6   �� 51      $��C#��6   �� 52      $��
Javascript��6   53      �� 54   
   -	   5        $��	CodeInput��54      $
��Left=�6'   $	��474��6B   $��
41,44,0,24q�6N   ��. $ �� HorizontalScrollBarVisibility$ Hidden=�6�   " �� VerticalScrollBarVisibility$
 Auto=�6�   $��0q�6�    �� AcceptsReturn . ���	#FF1F1C1C���	#FF0B0B0C$��1��6E  $��	Mono sans)�6R  
�������$��13.333(�6|   � 	Resources 56       �� 
TargetType �� Property �� Value��%X   'J�       �� 57      " J�6   .�58      ��  �@��Margin$ 0q�6/   59      5:      . �5;      /� .T�5<      J� 5=      5>      �� 5?   
   -
   5        $ label1��5?      $��
Brain Fuck��6    $��Right=�65   $��0,0,899,430q�6Q      SimpleLabel#���   6f   $��Bottom=�6�   $��84��6�   $��14.667(�6�   ���	#FF080707$��1��6�   �� 5@   
   -   5         ��Name$ 
CodeOutput��5@      $
��Left=�6*   $	��487��6E   $��515,44,0,24q�6Q   ��. $
 Auto=�6�   $
 Auto=�6�   ���	#FF2B2828$��0q�6�    . $��1q�6   �� 
IsReadOnly . ���	#FF0B0B0C$��1��6I  $��	Mono sans)�6V  
�������$��13.333(�6�  . �5A      /� .T�5B      J� 5C      5D      �� 5E   
   -   5        $��Lineinfo��5E      $!��줄:47   열:4   문자:500��6    
����������	#FF0B0B0C���	#FF252121$��
0,444,0,-2q�6�   $��맑은 고딕)�6�   $��1��6�   $��Right=�6�   $
��1002��6�   �� 5F   
   -   5         ��Name$ 번역��5F      $��번역��6   $
��Left=�6*   $��
432,19,0,0q�6E   $��1��6Y   $	��Top=�6f   $��75��6~   
���   �
�������$��1q�6�   
���   �6�   �� 5G   
   -   5        $ 번역_Copy��5G      $��복사��6"   $
��Left=�6/   $	��Top=�6J   $��67��6b   $��
925,18,0,0q�6�   $��1��6�   
���   �
�������6�   �� 5H   
   -   5        $ label1_Copy��5H      $��
Brain Fuck��6%   $��Right=�6:   $��0,0,899,430q�6V   #���   6k   $��Bottom=�6�   $��84��6�   $��14.667(�6�   ���	#FF080707$��1��6�   �� 5I   
   -   5        $ LineNumLabel��5I      ���	#FF252121$��Center=�6D   $��16,0,961,24q�6a   $��Bottom=�6v   $��25��6�   $��13.333(�6�   ���	#FFF17648$��1��6�   $��	Mono Sans)�6�   $	��400��6�   $��Center=�6�    . $��0q�6.  $ Hidden=�6B   5J      ��%X   'J�       �� 5K      " J�6   .�5L      ��  �@��Margin$ 0q�6/   5M      5N      . �5O      /� .T�5P      J� 5Q      5R      5S      5T      