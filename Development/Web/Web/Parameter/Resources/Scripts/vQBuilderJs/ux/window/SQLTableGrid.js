// SQLTableGrid --> Panel untuk nemampilkan table yang berisikan field2 pada halaman SQLTablePanel
Ext.define('Ext.ux.window.visualsqlquerybuilder.SQLTableGrid', {
    extend: 'Ext.grid.Panel',
    alias: ['widget.sqltablegrid'],
    border: false,
    selectedField: [],
    allfield: [],
    hideHeaders: true,
    listeners: {
        afterrender: function () {

            if (this.selectedField.length > 0) {

                var selmodel = this.selModel;
                var store = this.store;
                var selectedfiled = this.selectedField;
                setTimeout(function () {
                    for (var i = 0; i < selectedfiled.length; i++) {

                        selmodel.select($.grep(store.data.items, function (e) { return e.raw.field == selectedfiled[i].field; })[0].index, true);
                        
                    }
                }, 3000);
            }
        }
    },
    viewConfig: {
        listeners: {
            afterrender: function () {
                //                 
                //                this.selModel.select(1, true);
            },
            bodyscroll: function () {
                var scrollOffset, sqlTable;
                // the bodyscroll event of the view was fired
                // get scroll information
                scrollOffset = this.el.getScroll();
                // get the parent sqltable
                sqlTable = this.up('sqltable');
                // change shadowSprites scrollTop property
                sqlTable.shadowSprite.scrollTop = scrollOffset.top;
                // redraw all connections to reflect scroll action
                for (var i = ux.vqbuilder.connections.length; i--; ) {
                    sqlTable.connection(ux.vqbuilder.connections[i]);
                }
            },
            render: function (view) {
                this.dd = {};
                // init the view as a DragZone
                this.dd.dragZone = new Ext.view.DragZone({
                    view: view,
                    ddGroup: 'SQLTableGridDDGroup',
                    dragText: '{0} selected table column{1}',
                    onInitDrag: function (x, y) {
                        var me = this, data = me.dragData, view = data.view, selectionModel = view.getSelectionModel(), record = view.getRecord(data.item), e = data.event;
                        data.records = [record];
                        me.ddel.update(me.getDragText());
                        me.proxy.update(me.ddel.dom);
                        me.onStartDrag(x, y);
                        return true;
                    }
                });
                // init the view as a DropZone
                this.dd.dropZone = new Ext.grid.ViewDropZone({
                    view: view,
                    ddGroup: 'SQLTableGridDDGroup',
                    handleNodeDrop: function (data, record, position) {
                        // Was soll nach dem Drop passieren?
                    },
                    onNodeOver: function (node, dragZone, e, data) {
                        var me = this, view = me.view, pos = me.getPosition(e, node), overRecord = view.getRecord(node), draggingRecords = data.records;

                        if (!Ext.Array.contains(data.records, me.view.getRecord(node))) {
                            if (!Ext.Array.contains(draggingRecords, overRecord) && data.records[0].get('field') != '*') {
                                me.valid = true;
                                // valid drop target
                                // todo show drop invitation
                            }
                            else {
                                // invalid drop target
                                me.valid = false;
                            }
                        }
                        return me.valid ? me.dropAllowed : me.dropNotAllowed;
                    },
                    onContainerOver: function (dd, e, data) {
                        var me = this;
                        // invalid drop target
                        me.valid = false;
                        return me.dropNotAllowed;
                    }
                });
            },
            drop: function (node, data, dropRec, dropPosition) {

                var sqlTable1, sqlTable2, showJoinCM, connection, aBBPos, join, joinCondition = '', dropTable, targetTable;

                showJoinCM = function (event, el) {
                    var cm;
                    // stop the browsers event bubbling
                    event.stopEvent();
                    // create context menu
                    cm = Ext.create('Ext.menu.Menu', {
                        items: [{
                            text: 'Edit Join',
                            icon: '/Resources/img/vQBuilderImg/document_edit16x16.gif',
                            handler: Ext.Function.bind(function () {

                            }, this)
                        }, {
                            text: 'Remove Join',
                            icon: '/Resources/img/vQBuilderImg/remove.gif',
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
                            icon: '/Resources/img/vQBuilderImg/cross.gif',
                            handler: Ext.emptyFn
                        }]
                    });
                    // show the contextmenu next to current mouse position
                    cm.showAt(event.getXY());
                };

                if (node.boundView) {

                    sqlTable1 = data.view.up('window');
                    sqlTable1.shadowSprite.bConnections = true;

                    sqlTable2 = Ext.getCmp(node.boundView).up('window');
                    sqlTable2.shadowSprite.bConnections = true;

                    dropTable = ux.vqbuilder.sqlSelect.getTableById(sqlTable1.tableId);
                    targetTable = ux.vqbuilder.sqlSelect.getTableById(sqlTable2.tableId);

                    aBBPos = [data.item.viewIndex, node.viewIndex];

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
                    join.set('leftTableField', data.records[0].get('field'));
                    // sqlTable1 is the left table
                    join.set('rightTableId', sqlTable2.tableId);
                    // node.viewIndex is the index of the target node
                    join.set('rightTableField', sqlTable2.down('grid').store.getAt(node.viewIndex).get('field'));
                    // set the defaul join type to INNER
                    join.set('joinType', 'INNER');

                    if (dropTable.get('tableAlias') != '') {
                        joinCondition = joinCondition + dropTable.get('tableAlias') + '.' + join.get('leftTableField') + '=';
                    }
                    else {
                        joinCondition = joinCondition + dropTable.get('tableName') + '.' + join.get('leftTableField') + '=';
                    }

                    if (targetTable.get('tableAlias') != '') {
                        joinCondition = joinCondition + targetTable.get('tableAlias') + '.' + join.get('rightTableField');
                    }
                    else {
                        joinCondition = joinCondition + targetTable.get('tableName') + '.' + join.get('rightTableField');
                    }

                    join.set('joinCondition', joinCondition);
                    ux.vqbuilder.sqlSelect.addJoin(join);
                }

            }
        }
    },
    initComponent: function () {

        this.columns = [{
            xtype: 'gridcolumn',
            width: 16,
            dataIndex: 'key',
            renderer: function (val, meta, model) {
                if (val == 'PRI') {
                    meta.style = 'background-image:url(/Resources/Img/vQBuilderImg/key.gif) !important;background-position:2px 3px;background-repeat:no-repeat;';
                }
                return '&nbsp;';
            }
        }, {
            xtype: 'gridcolumn',
            flex: 1,
            dataIndex: 'field',
            renderer: function (val, meta, model) {
                if (model.get('key') == 'PRI') {
                    return '<span style="font-weight: bold;">' + val + '</span>&nbsp;&nbsp;<span style="color:#aaa;">' + model.get('type') + '</span>';
                }
                return val + '&nbsp;&nbsp;<span style="color:#999;">' + model.get('type') + '</span>';

            }
        }];

        this.selModel = Ext.create('Ext.selection.CheckboxModel', {
            mode: 'SIMPLE',
            checkOnly: true,
            listeners: {
                select: function (selModel, data) {
                    
                    // add new rows to the SQLFieldsGrid after a selection change
                    ux.vqbuilder.sqlSelect.addFieldRecord(data, true);
                },
                deselect: function (selModel, data) {
                    var store, model;
                    // remove row from SQLFieldsGrid after deselection
                    ux.vqbuilder.sqlSelect.removeFieldById(data.get('id'));
                }
            }
        });
        //        window.SelModel.push(this.selModel);
        this.callParent(arguments);
    }
});
// ----------------------------------------------------------