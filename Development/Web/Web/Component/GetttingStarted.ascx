<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GetttingStarted.ascx.vb" Inherits="Component_WebUserControl" %>

<style>
    .img-chooser-view .thumb-wrap {
        margin: 5px !important;
    }

    .img-chooser-view .thumb-wrap:hover {
        cursor: pointer;
    }

    .img-chooser-view .thumb-wrap:active {
        color: #1A4D8F;
    }

    .NawaLabel {
        max-width: 105px;
    }
</style>

<ext:Container ID="Container1" runat="server" AnchorHorizontal="100%" ClientIDMode="Static">
    <Items>
        <ext:DataView runat="server" SingleSelect="true" ID="dataview1" Cls="img-chooser-view" OverItemCls="x-view-over"
            ItemSelector="div.thumb-wrap" ClientIDMode="Static">
            <Store>
                <ext:Store ID="Store1" runat="server">        
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="StartedName" />
                                <ext:ModelField Name="Iconfile" />    
                                <ext:ModelField Name="ModuleID" />    
                                <ext:ModelField Name="ModuleAction" />    
                                <ext:ModelField Name="Urldata" />          
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>      
            </Store>
            <Tpl>
                <Html>
                    <tpl for=".">
                        <div class="thumb-wrap">
                            <div class="thumb">
                                <tpl if="!Ext.isIE6">
                                    <img src="data:image/jpg;base64,{Iconfile}" width="100" height="100" style="border-radius:10px;"/>
                                </tpl>
                                <tpl if="Ext.isIE6">
                                    <div style="width:74px;height:74px;border-radius:10px;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src="data:image/jpg;base64,{Iconfile}")"></div>
                                </tpl>                    
                            </div>
                            <span class="NawaLabel">{StartedName}</span>
                        </div>
                    </tpl>
                </html>
            </Tpl>
            <Listeners>
                <SelectionChange Handler="Ext.net.Mask.show(); NawadataDirect.RedirectData(selected[0].data);"  />
            </Listeners>
        </ext:DataView>
    </Items>
</ext:Container>

