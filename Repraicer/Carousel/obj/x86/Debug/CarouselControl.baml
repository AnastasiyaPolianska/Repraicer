   M S B A M L   `   `   `  ����j  fExpression.Samples.PathListBoxUtils, Version=4.0.0.0, Culture=neutral, PublicKeyToken=76319f7ba805fb35�^clr-namespace:Expression.Samples.PathListBoxUtils;assembly=Expression.Samples.PathListBoxUtils#Expression.Samples.PathListBoxUtils  5      C ?Carousel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CarouselCarousel 5         Carousel.CarouselControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      {PathListBoxUtils^clr-namespace:Expression.Samples.PathListBoxUtils;assembly=Expression.Samples.PathListBoxUtils     5      d `Microsoft.Expression.Controls, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35>ec5http://schemas.microsoft.com/expression/2010/controls  5      c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      "localclr-namespace:Carousel  5        � 	Resources  5          navigatorPreviousButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name J� Opacity �� Triggers P� Property	 P� Value3  -System.Windows.Media.Effects.DropShadowEffect
   	Direction   Opacity   ShadowDepth   Color �� 
TargetName  navigatorNextButtonStyleI   CExpression.Samples.PathListBoxUtils.AdjustedOffsetToRadiusConverter # AdjustedOffsetToRadiusConverter  PathListBoxItemStyle3  -Microsoft.Expression.Controls.PathListBoxItem J� RenderTransformOrigin �� ScaleY �� Path �� RelativeSource �� 	Converter>  8Microsoft.Expression.Controls.IsArrangedToScaleConverter �� ScaleX �� AngleD   >Expression.Samples.PathListBoxUtils.PathListBoxItemTransformer  Name   
IsCentered   Ease-  'System.Windows.Media.Animation.SineEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode �Name ��Name �� RadiusX �� RadiusY ��Name J� IsHitTestVisible'  !System.Windows.VisualStateManager    VisualStateGroups%	  System.Windows.VisualStateGroup! 	  States 
  System.Windows.VisualState��%�  &	        &	     &	      &	 3    �� 5   
   " ��65   .�5      ��  � ��$ 0q�6(   �� 5       � ��$ 0q�6'   �� 5       �@�FocusVisualStyle 61   Z� �� 5       � �� 5      �� 5      " ��6&   .R�5      (� $��40��6"   $��40��6-   .��5      �� $	 img��5   !   #��z��5   !   $	 0.5Q�5   !   $��Images/previous.png��5   !   $��Uniform=�5   !   5       5      P� 5       �@J�IsMouseOver	 . . �5   "   ��  �@J�Effect 5    &     5!   *   $	
 315Q�5"   -   $	 0.5Q�5#   -   $ 2Q�5$   -   $	 Red��5%   -   5'   #   �� 5(   "   $	 img��6)    �@J�Opacity$	 1.0Q�6M   5)      P� 5*       � {�	 .  . �5+   "   �� $	 img��6)    �@J�Opacity$	 0.3Q�6M   5,      5.      50      51      �� 54   
   " ��61   .�55      ��  � ��$ 0q�6(   �� 56       � ��$	 0,5q�6'   �� 57       �@�FocusVisualStyle 61   Z� �� 58       � �� 59      �� 5:      " ��6&   .R�5;      (� $��40��6"   $��40��6-   .��5<      �� $	 img��5=   !   #��z��5>   !   $	 0.5Q�5?   !   $��Images/next.png��5@   !   $��Uniform=�5A   !   5B       5C      P� 5D       �@J�IsMouseOver	 . . �5E   "   ��  �@J�Effect 5F   &     5G   *   $	
 315Q�5H   -   $	 0.5Q�5I   -   $ 2Q�5J   -   $	 Red��5K   -   5M   #   �� 5N   "   $	 img��6)    �@J�Opacity$	 1.0Q�6M   5O      P� 5P       � {�	 .  . �5Q   "   �� $	 img��6)    �@J�Opacity$	 0.3Q�6M   5R      5T      5V      5W        5Z   
   6;   �� 5]   
   "  6-   .�5^      ��  � ��$ Center=�6;   �� 5_       � ��$	 Top=�69   �� 5`       � ��
 ���� 6+   �� 5a       � ��$ 1q�60   �� 5b       �@�FocusVisualStyle 61   Z� �� 5c       � �� 5d      �� 5e      "  6&   .R�5f      � #��z��6   $ 0.5,0.5&�6I   z�5g      V� 5h   "   .~�5i   &   ��  5j   *   �� 5k   .   $ 
IsArranged�66    6H   �� *TemplatedParent+ 5l   2     5m   6   5o   /    5q   *   �� 5r   .   $ 
IsArranged�66    6H   �� *TemplatedParent+ 5s   2     5t   6   5v   /   5x   '   �� 5y   &   �� 5z   &    66   �� *OrientationAngle+ �� *TemplatedParent+T� 5{   &   5|   #   .+�5~        $  pathListBoxItemTransformer��5   !   -   5        $	��Top=�5�   !    .  5�   "     5�   &   $ EaseIn=�6/   .��5�   "   � $ TransformerParentGrid��6'   $
��Auto��6F   .+�5�   &   �� $ FocusVisualElement��5�   )   $ 1��5�   )   $ 1��5�   )   ���	#FF6DBDD1$��1��5�   )   $y�	Collapsed=�5�   )   �� 5�   &   $ contentPresenter��5�   )   #��z��5�   )   #��z��5�   )   #��z��5�   )   #��z��5�   )   �� 5�   &   $ 	fillColor��5�   )   ���	#FFBADDE9 .  $ 0Q�5�   )   $ 1��5�   )   $ 1��5�   )   �� 5�   &   $ 
fillColor2��5�   )   ���	#FFBADDE9 .  $ 0Q�5�   )   $ 1��5�   )   $ 1��5�   )   5�   #   5�        5�      	  5�   "   CommonStates��.! 5�   &   
  
Normal��62   
  5�   &   	MouseOver��62   
  5�   &   Disabled��62   5�   #   	  5�   "   SelectionStates��.! 5�   &   
  
Unselected��62   
  5�   &   Selected��62   5�   #   	  5�   "   FocusStates��.! 5�   &   
  Focused��62   
  5�   &   	Unfocused��62   5�   #   5�      5�      5�      5�      5�      .��5�      � $��0q�6   " � ColumnDefinitions" 5�   
   �� 5�      $��50 �6   �� 5�      $��* �6   �� 5�      $��50 �6   .+�5�   
   �� -   5        # ��Name$# previousButton��5�      $��0��5�      $��<��5�      #��[  5�      /  )Microsoft.Expression.Controls.PathListBox  5�   
   -   5        $  Name$$ pathListBox��5�      $��1��5�      $��	40,0,40,0q�5�      #���  5�      % �� SelectionMode$% Single=�5�      &   	WrapItems& . '   LayoutPaths' 5�      .  (Microsoft.Expression.Controls.LayoutPath  5�      (   Capacity?  9Microsoft.Expression.Controls.LayoutPathCapacityConverter$( 7 5�      )   Distribution$
) Even=�5�      *   FillBehavior$* 	NoOverlap=�5�      +   Start$
+ 0.01Q�5�      ,   SourceElement, 5�      �� - �� ElementName$	- ell��5�      .  (System.Windows.Interactivity.Interaction.   	Behaviors. 5�      C   =Expression.Samples.PathListBoxUtils.PathListBoxScrollBehavior  5�      /   DesiredOffset$	/ 0.5Q�6=   0   HideEnteringItem0 .  1   Ease1 5�        5�      $ EaseOut=�6#   2   Triggers2 5�      /  )System.Windows.Interactivity.EventTrigger  5�      3   	EventName$3 Click��5�      3  -System.Windows.Interactivity.EventTriggerBase4   
SourceName$4 pathListBox��5�      5   SourceObject5 5�      �� $- previousButton��5�        5�      $3 Click��5�      $4 pathListBox��5�      5 5�      �� $- 
nextButton��5�        5�      $3 SelectionChanged��6)   $4 pathListBox��6F   .  (System.Windows.Interactivity.TriggerBase6   Actions.6 5�      6  0System.Windows.Interactivity.InvokeCommandAction  7   CommandName$7 ScrollSelectedCommand��64   5�      5�      5�      �� 5�   
   -   5        $# 
nextButton��5�      $��2��5�      $��>��5�      #��[ 5�      � 5�   
   -   5        $ gridForKnownPaths��5�      $��1��5�      $��Stretch=�5�      $��Stretch=�5�      .+�5�      F� -   5        8 F�Name$8 wavePath��5�      $��-46,127,-31,106q�5�      ������ZQ������D����@���d%@���d%р�� z
� ��mb� ��mb����#�J@�Q� � I@; �0=@; �0=�@� ��@vX#�o�р?�& �*	9 �� Stretch$
9 Fill=�5      
������ $��1��5     F� 5     -   5        $8 diagonalPath��5     $��-44,79,14,-31q�5     4����@�o� '�@p�@g�� ̿ �C� �x�SB�@�L$ &"	$
9 Fill=�5     
������ $��1��5	     F� 5
     -   5        $8 
zigzagPath��5     $��-38,32.5,-26.5,16q�5     ����Ѐ*��@�TӀ&i��#�1�� � y� um�`�� U� �I CR �I CR��dy�-t�`L7�0� �~`�� O����/� ���`��&��	$
9 Fill=�5     
������ $��1��5     A� 5     -	   5        : A�Name$: ellipsePath��5     $��40,32.5,82.5,106q�5     
������ $��1��5     5     5     5     