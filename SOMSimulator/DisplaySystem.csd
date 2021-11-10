<ClassProject>
  <Language>CSharp</Language>
  <Entities>
    <Entity type="Class">
      <Name>DisplayPanel</Name>
      <Access>Public</Access>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Class">
      <Name>DisplayArea</Name>
      <Access>Public</Access>
      <Member type="Property">public int NumberOfPanels { get; set; }</Member>
      <Member type="Property">public int SelectedPanel { get; set; }</Member>
      <Member type="Event">public event EventHandler SelectedPanelChanged</Member>
      <Member type="Event">public event EventHandler PanelClicked</Member>
      <Modifier>None</Modifier>
    </Entity>
    <Entity type="Interface">
      <Name>IVisualiser</Name>
      <Access>Public</Access>
    </Entity>
    <Entity type="Class">
      <Name>TableLayoutPanel</Name>
      <Access>Internal</Access>
      <Modifier>None</Modifier>
    </Entity>
  </Entities>
  <Relations>
    <Relation type="Association" first="0" second="1">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>True</IsComposition>
    </Relation>
    <Relation type="Association" first="2" second="0">
      <Direction>None</Direction>
      <IsAggregation>False</IsAggregation>
      <IsComposition>False</IsComposition>
    </Relation>
    <Relation type="Generalization" first="1" second="3" />
  </Relations>
  <Positions>
    <Shape>
      <Location left="636" top="252" />
      <Size width="162" height="117" />
    </Shape>
    <Shape>
      <Location left="340" top="252" />
      <Size width="207" height="153" />
    </Shape>
    <Shape>
      <Location left="907" top="280" />
      <Size width="162" height="83" />
    </Shape>
    <Shape>
      <Location left="340" top="55" />
      <Size width="162" height="114" />
    </Shape>
    <Connection>
      <StartNode isHorizontal="True" location="69" />
      <EndNode isHorizontal="True" location="69" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="True" location="40" />
      <EndNode isHorizontal="True" location="68" />
    </Connection>
    <Connection>
      <StartNode isHorizontal="False" location="81" />
      <EndNode isHorizontal="False" location="77" />
    </Connection>
  </Positions>
</ClassProject>