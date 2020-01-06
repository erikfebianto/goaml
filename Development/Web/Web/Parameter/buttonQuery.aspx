<%@ Page Language="VB" AutoEventWireup="false" CodeFile="buttonQuery.aspx.vb" Inherits="buttonQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="Resources/CSS/vQBuilderCss/app.css" rel="stylesheet" type="text/css" />
    <link href="Resources/CSS/vQBuilderCss/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="Resources/CSS/vQBuilderCss/shCore.css" rel="stylesheet" type="text/css" />
    <link href="Resources/CSS/vQBuilderCss/shThemeDefault.css" rel="stylesheet" type="text/css" />
    
    
    <script type="text/JavaScript" src="Resources/Scripts/vQBuilderJs/ext-all.js"></script>    
    <script type="text/JavaScript" src="Resources/Scripts/jquery-1.9.0.min.js"></script>
    <script type="text/JavaScript" src="Resources/Scripts/bootstrap.min.js"></script>
    <script src="Resources/Scripts/vQBuilderJs/shCore.js"></script>
    <script src="Resources/Scripts/vQBuilderJs/shBrushSql.js"></script>
   </head>
<body>
    <form id="form1" runat="server">
    <div>
    <button id="showvqb" class="btn btn-success" onclick="return false;">Open Designer</button>
        <%--<button onclick="ShowAdvancedQuery();return false;" class="btn btn-success">Use Advanced Query</button>   --%>
        <br/>
        
          <input type="hidden" id="ViewDesignerQuery" runat="server"
                               Name ="ViewDesignerQuery" 
                        ></input>

                    <pre id="vqbQuery" style="min-height: 300px; display: none" class="brush:sql" name="vqbQuery"></pre>
    </div>
    </form>
</body>
    
<script type="text/javascript">
    
    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    function CloseWindow() {
        if (window.opener != null && !window.opener.closed) {
            if (getParameterByName('object') == '1') {

                window.opener.App.txtqueryEmail.setValue(document.getElementById("ViewDesignerQuery").value);
                window.opener.App.hQueryObjectDesignerEmail.setValue(JSON.stringify(orgnObj));
            }
            else {
                window.opener.App.txtquery.setValue(document.getElementById("ViewDesignerQuery").value);
                window.opener.App.hQueryObjectDesigner.setValue(JSON.stringify(orgnObj));
            }
            
            
            
        }
        window.close();
    }

    function ShowHideQuery() {
        jQuery('#ViewDesignerQuery').hide();
        jQuery('#ViewDesignerQuery').next().remove();
        jQuery('#ViewDesignerQuery').after('<pre id="vqbQuery" style="min-height: 300px" class="brush:sql" name="vqbQuery">' + jQuery('#ViewDesignerQuery').val() + '</pre>');
        SyntaxHighlighter.defaults['gutter'] = false;
        SyntaxHighlighter.defaults['toolbar'] = false;
        SyntaxHighlighter.highlight();
    }

    function ShowAdvancedQuery() {
        jQuery('#ViewDesignerQuery').next().hide();
        jQuery('#ViewDesignerQuery').show();
        jQuery('#ViewDesignerQuery').focus();
    }
   
    function ShowVisualQueryBuilder() {
        _rootUrl = '<%= GetRootURL()%>';  //"/NawaTemplate/parameter/";
        var currentUrl = _rootUrl.substr(1);
        
        UseQueryDesigner = true;
        // Init the singleton.  Any tag-based quick tips will start working.
        Ext.tip.QuickTipManager.init();

        // create main application namespace ux.qb.app
        Ext.namespace('ux.qb.app');
         
        // configure Ext.Loader
        Ext.Loader.setConfig({
            enabled: true,
            paths: {
                Ext: currentUrl  + 'Resources/Scripts/vQBuilderJs'
            }
        });

        Ext.application({
            name: 'ux.qb',
            appFolder: 'app',
            autoCreateViewport: false,
            launch: function () {
                // copy application to ux.qb.app so that ux.qb.app can be used as an application singleton 
                var qbWindow = Ext.create('Ext.ux.window.VisualSQLQueryBuilder');
                console.log(qbWindow);
                qbWindow.show();
                Ext.apply(ux.qb.app, this);
            }
        });


        ObjQuery = jQuery.extend(true, {}, orgnObj);
        
        if (ObjQuery.tables != undefined && ObjQuery.tables.length > 0) {
            table2 = [];

            $.each(ObjQuery.tables, function (i, val) {

                var selectedField = jQuery.grep(ObjQuery.fields, function (n, i) {
                    return (n.tableId == val.id);
                });

                table2.push({
                    xtype: 'sqltable',
                    constrain: true,
                    title: val.tableName,
                    titleAlias: val.tableAlias,
                    id: val.id,
                    selectedField: selectedField
                });
                 
            });
             
            
            join3 = [];
            table3 = [];
            s2 = Ext.getCmp('SQLTablePanel');
            $.each(table2, function (i, val) {
                
                table3.push(s2.add({
                    xtype: 'sqltable',
                    constrain: true,
                    title: val.title,
                    titleAlias: val.titleAlias,
                    selectedField: val.selectedField
                }).show());
                
                $.each(ObjQuery.joins, function (j, val2) {
                    if (val2.leftTableId == val.id) {
                        val2.leftTableId = table3[i].tableId;
                    }
                    if (val2.rightTableId == val.id) {
                        val2.rightTableId = table3[i].tableId;
                    }
                });

            });

            buildjoin = [];
            $.each(ObjQuery.joins, function (i, val) {
                buildjoin.push({
                    leftTable: jQuery.grep(table3, function(n, i) { return (n.tableId == val.leftTableId); })[0],
                    rightTable: jQuery.grep(table3, function(n, i) { return (n.tableId == val.rightTableId); })[0],
                    leftTableField: val.leftTableField,
                    rightTableField: val.rightTableField,
                    joinType: val.joinType,
                    joinCondition:val.joinCondition
                }
                );
            });
            setTimeout(function () {
                $.each(buildjoin, function (i, val) {
                    createJoin(
                        val.leftTable,
                        val.rightTable,
                        val.leftTableField,
                        val.rightTableField,
                        val.joinType,
                        val.joinCondition
                    );
                });
            }, 6000);

        }
    }

    $(function () {
    
      
        //if (window.App._Querydata


        

        orgnObj = {};
        try {

            if (getParameterByName('object') == '1') {

                if (window.opener.App.hQueryObjectDesignerEmail.value != '') {

                    orgnObj = JSON.parse(window.opener.App.hQueryObjectDesignerEmail.value);

                }
                else {
                    orgnObj = {};
                }

            }
            else {


                if (window.opener.App.hQueryObjectDesigner.value != '') {

                    orgnObj = JSON.parse(window.opener.App.hQueryObjectDesigner.value);

                }
                else {
                    orgnObj = {};
                }
            }
            
            
        } catch(e) {
            orgnObj = {};
        }
        
        


        ShowHideQuery();
        

        $('#showvqb').click(function () {
            
            
                ShowVisualQueryBuilder();
           


        });
    });
</script>

<script type="text/javascript">
    Ext.define('Ext.ux.window.visualsqlquerybuilder.JCEditor', {
        extend: 'Ext.window.Window',
        alias: ['widget.jceditor'],
        cascadeOnFirstShow: 20,
        join:{},
        height: 0,
        width: 200,
        layout: {
            type: 'fit'
        },
        closable: false,
        header: false,
        listeners: {
            show: function() {
            },
            beforeclose: function() {
            },
            afterrender: function() {
            }
        },
        initComponent: function() {
            var thisJCEditor = this;
            var thisjoin = this.join;
            this.items = [{
                xtype: 'textfield',
                initComponent: function() {
                    
                    this.setValue(thisjoin.data.joinCondition);
                    this.on('specialkey', function(field, event) {
                        if (event.getKey() == event.ENTER) {
                            
                            ux.vqbuilder.sqlSelect.updateJoinCondition(thisjoin.data.id, this.getValue());
                            thisJCEditor.close();
                        }
                    }, this);

                    this.on('blur', function(field, event) {
                        
                        ux.vqbuilder.sqlSelect.updateJoinCondition(thisjoin.data.id, this.getValue());
                        thisJCEditor.close();
                    }, this);

                    this.callParent(arguments);
                }
            }];

            this.callParent(arguments);
        },
    });
    
    $(function () {
        showJoinCM = function (event, el) {
            var cm;
            // stop the browsers event bubbling
            event.stopEvent();
            // create context menu
            cm = Ext.create('Ext.menu.Menu', {
                items: [{
                    text: 'Edit Join',
                    icon: 'Resources/img/vQBuilderImg/document_edit16x16.gif',
                    menu: {
                        xtype: 'menu',
                        items: [{
                            text: 'INNER JOIN',
                            icon: 'Resources/img/vQBuilderImg/InnerJoin.png',
                            handler: Ext.Function.bind(function () {
                                // remove any connection lines from surface and from array ux.vqbuilder.connections
                                ux.vqbuilder.sqlSelect.updateJoin(this.uuid, 'INNER');
                            }, this)
                        }, {
                            text: 'LEFT JOIN',
                            icon: 'Resources/img/vQBuilderImg/LeftJoin.png',
                            handler: Ext.Function.bind(function () {
                                // remove any connection lines from surface and from array ux.vqbuilder.connections
                                ux.vqbuilder.sqlSelect.updateJoin(this.uuid, 'LEFT');
                            }, this)
                        }, {
                            text: 'RIGHT JOIN',
                            icon: 'Resources/img/vQBuilderImg/RigthJoin.png',
                            handler: Ext.Function.bind(function () {
                                // remove any connection lines from surface and from array ux.vqbuilder.connections
                                ux.vqbuilder.sqlSelect.updateJoin(this.uuid, 'RIGHT');
                            }, this)
                        }, {
                            text: 'FULL JOIN',
                            icon: 'Resources/img/vQBuilderImg/FullJoin.png',
                            handler: Ext.Function.bind(function () {
                                // remove any connection lines from surface and from array ux.vqbuilder.connections
                                ux.vqbuilder.sqlSelect.updateJoin(this.uuid, 'FULL');
                            }, this)
                        }]
                    },
                    handler: Ext.Function.bind(function () {

                    }, this)
                }, {
                    text: 'Join Condition',
                    icon: 'Resources/img/vQBuilderImg/document_edit16x16.gif',
                    handler: Ext.Function.bind(function () {
                        var thisjoin = ux.vqbuilder.sqlSelect.getJoin(this.uuid);
                             
                        var sqlpanel = Ext.getCmp('SQLTablePanel');
                        sqlpanel.add({
                            xtype: 'jceditor',
                            constrain: true,
                            join : thisjoin,
                            joinid:this.uuid
                        }).show();                     
                    }, this)
                }, {
                    text: 'Remove Join',
                    icon: 'Resources/img/vQBuilderImg/remove.gif',
                    handler: Ext.Function.bind(function () {
                        // remove any connection lines from surface and from array ux.vqbuilder.connections
                        ux.vqbuilder.connections = Ext.Array.filter(ux.vqbuilder.connections, function (connection) {
                            var bRemove = true;
                            if (this.uuid == connection.uuid) {
                                this.line.remove();
                                this.bgLine.remove();
                                this.miniLine1.remove();
                                this.miniLine2.remove();
                                bRemove = false;
                            }
                            return bRemove;
                        }, this);
                        ux.vqbuilder.sqlSelect.removeJoinById(this.uuid);
                    }, this)
                }, {
                    text: 'Close Menu',
                    icon: 'Resources/img/vQBuilderImg/cross.gif',
                    handler: Ext.emptyFn
                }]
            });
            // show the contextmenu next to current mouse position
            cm.showAt(event.getXY());
        };

        createJoin = function (sqlTable1, sqlTable2, field1, field2, joinType,joinCondition) {

            //            joinCondition = ''

            index1 = $.grep(sqlTable1.down('grid').store.data.items, function (n, i) { return n.data.field == field1; })[0].index;
            index2 = $.grep(sqlTable2.down('grid').store.data.items, function (n, i) { return n.data.field == field2; })[0].index;

            sqlTable1.shadowSprite.bConnections = true;

            sqlTable2.shadowSprite.bConnections = true;

            dropTable = ux.vqbuilder.sqlSelect.getTableById(sqlTable1.tableId);
            targetTable = ux.vqbuilder.sqlSelect.getTableById(sqlTable2.tableId);

            aBBPos = [index1, index2];

            connection = sqlTable2.connection(sqlTable1.shadowSprite, sqlTable2.shadowSprite, "#000", aBBPos);

            sqlTable1.connectionUUIDs.push(connection.uuid);
            sqlTable2.connectionUUIDs.push(connection.uuid);

            ux.vqbuilder.connections.push(connection);

            // bgLine is white(invisble) and its stroke-width is 10
            // so it is easier to capture the dblclick event
            connection.bgLine.el.on('contextmenu', showJoinCM, connection);

            // line is black and its stroke-width is 1
            connection.line.el.on('contextmenu', showJoinCM, connection);

            // create an instance of the join model
            join = Ext.create('Ext.ux.window.visualsqlquerybuilder.SQLJoin');
            // set join id
            join.set('id', connection.uuid);
            // sqlTable1 is the left table
            join.set('leftTableId', sqlTable1.tableId);
            // data.records[0] represents the model of the dragged node
            join.set('leftTableField', sqlTable1.down('grid').store.getAt(index1).get('field'));
            // sqlTable1 is the left table
            join.set('rightTableId', sqlTable2.tableId);
            // node.viewIndex is the index of the target node
            join.set('rightTableField', sqlTable2.down('grid').store.getAt(index2).get('field'));
            // set the defaul join type to INNER
            join.set('joinType', joinType);

            //            if (dropTable.get('tableAlias') != '') {
            //                joinCondition = joinCondition + dropTable.get('tableAlias') + '.' + join.get('leftTableField') + '=';
            //            } else {
            //                joinCondition = joinCondition + dropTable.get('tableName') + '.' + join.get('leftTableField') + '=';
            //            }

            //            if (targetTable.get('tableAlias') != '') {
            //                joinCondition = joinCondition + targetTable.get('tableAlias') + '.' + join.get('rightTableField');
            //            } else {
            //                joinCondition = joinCondition + targetTable.get('tableName') + '.' + join.get('rightTableField');
            //            }

            join.set('joinCondition', joinCondition);
            ux.vqbuilder.sqlSelect.addJoin(join);
        };
    })
</script>

<script type="text/javascript" >
  

        $('#btnPreview').click(function () {

            //            $('#bModal').attr("style", "display:none");
            //            $('#bModal').find('.modal-header').find('.isw-grid').remove();
            //            $('#bModal').find('.modal-header').children().last().text('Pemberitahuan !!!');
            //            $('#bModal').find('.modal-footer').show();
            // alert("a");
            var dataparameter = $('#frmViewDesigner').serializeObject();

            $.ajax({
                url:
                    '@VirtualPathUtility.ToAbsolute("~/QueryBuilderData.aspx?GetColumnadvancequery=1")',
                data: JSON.stringify(dataparameter),
                type: 'post',
                contentType: 'application/Json',
                success: function (hasil, _statusMessage, _xhrObject) {
                    if (hasil.Success) {
                        $('#dg2').remove();
                        $('#frmViewDesigner').append('<table cellpadding="0" cellspacing="0" width="120%" class="table" id="dg2"></table>');

                        $('#dg2').datagrid({
                            queryParams: {
                                query: $('#ViewDesignerQuery').val(),
                                orderBy: hasil.orderBy
                            },
                            height: 330,
                            rownumbers: true,
                            nowrap: true,
                            pagination: true,
                            pagePosition: 'bottom', /*top, bottom, both*/
                            pageSize: 10, /* Initialize the page size */
                            pageList: [10, 25, 50, 100],
                            /*striped: true,*/
                            singleSelect: true,
                            showHeader: true,
                            columns: [hasil.columns],
                            url:
                                '@VirtualPathUtility.ToAbsolute("~/QueryBuilderData.aspx/GetData=1")'
                        });
                        $('#dg2').datagrid('getPager').pagination({ showPageList: false });
                        var grid = $('#dg2').parent().parent().parent();
                        
                        //                        $('#bModal').find('.modal-header').find('.isw-grid').remove();
                        //                        $('#bModal').find('.modal-header').children().last().before('<div class="isw-grid" style="margin-right: 11px ;margin-left: 0px;"></div>');
                        //                        $('#bModal').find('.modal-header').children().last().text('Preview Data View Designer');
                        //                        $('#bModal').find('.modal-body').html(grid);
                        //                        $('#bModal').find('.modal-footer').hide();

                        //                        $('#bModal').find('.modal-body').css('padding', '0px');

                        //                        $('#bModal').css('width', '70%');
                        //                        $('#bModal').css('left', '40%');
                        //                        $('#bModal').modal('show');
                        ShowModal(
                            '70%',
                            '40%',
                            BuildHeader('Preview Data View Designer', 'isw-grid'),
                            grid,
                            BuildFooter(false),
                            true
                        );
                        grid.children().css('width', 'auto');
                        grid.children().children().first().css('width', 'auto');

                        $('#dg2').datagrid('load');

                    } else {
                        msgOnUpdate(hasil);
                    }


                },
                error: function (xhr, status) {
                    switch (xhr.status) {
                        case 403:
                            ShowMessages("Saat ini anda tidak mempunyai hak untuk melakukan proses pencatatan transaksi berikut ini<br>Silahkan login ulang bila anda memang mempunyai hak akses untuk melakukan pencatatan transaksi");
                            break;
                        case 404:
                            ShowMessages("Saat anda melakukan proses pencatatan transaksi modul ini tidak ditemukan");
                            break;
                        case 500:
                            ShowMessages("Saat anda melakukan proses pencatatan transaksi terjadi kesalahan aplikasi di server , silahkan kontak administrator");
                            break;
                        case 405:
                            ShowMessages("Saat anda melakukan proses pencatatan transaksi proses tidak diijinkan , silahkan kontak administrator");
                            break;
                        default:
                            ShowMessages("Error!!!...")
                            break;
                    }
                }
            });

            return false;

        });
    
</script>
</html>
