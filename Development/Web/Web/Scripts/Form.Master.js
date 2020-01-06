


var waitConfirm = false;
var resConfirm = false;

var template = '<span style="color:{0};">{1}</span>';


Ext.override(Ext.grid.RowNumberer, { resizable: true });

Ext.override(Ext.form.Panel , {
    fieldDefaults: { labelSeparator: "</span><div class='x-form-item-label-seperator'>:</div><span>", labelWidth: 150, msgTarget :"side"}
}
);



Ext.override(Ext.form.Field, {
    fireKey: function (e) {
        if (e.keyCode == e.ENTER) {
            e.stopEvent();
        }
        this.fireEvent('keypress', this, e);
        if (e.isSpecialKey())
            this.fireEvent('specialkey', this, e);
        this.fireEvent('keydown', this, e);
        this.fireEvent('keyup', this, e);
    },

    setCursorPosition: function (pos) {
        var el = this.inputEl.dom;
        if (typeof (el.selectionStart) === "number") {
            el.focus();
            el.setSelectionRange(pos, pos);
        } else if (el.createTextRange) {
            var range = el.createTextRange();
            range.move("character", pos);
            range.select();
        } else {
            throw 'setCursorPosition() not supported';
        }
    },

    getCursorPosition: function () {
        var el = this.inputEl.dom;
        if (typeof (el.selectionStart) === "number") {
            return el.selectionStart;
        } else if (document.selection && el.createTextRange) {
            var range = document.selection.createRange();
            range.collapse(true);
            range.moveStart("character", -el.value.length);
            return range.text.length;
        } else {
            throw 'getCursorPosition() not supported';
        }
    }
});

Ext.override(Ext.form.field.Number, {
    currencySymbol: null,
    useThousandSeparator: true,
    alwaysDisplayDecimals: true,
    hideTrigger: true,
    fieldStyle: 'text-align: right;',
    enableKeyEvents: true,
    onKeyUp: function (e, o) {
        if (this.getRawValue() != this.getFormattedValue(this.getRawValue())) {
            posbef = this.getCursorPosition();
            lengthbef = this.getRawValue().length;
            this.setRawValue(this.getFormattedValue(this.getRawValue()));
            this.setCursorPosition(posbef + this.getRawValue().length - lengthbef);

        }
    },
    setValue: function (value) {
        if (value == 0) {
            this.setRawValue("0");
        }
        else {
            this.setRawValue(this.getFormattedValue(value));
        }
        
    },
    getFormattedValue: function (value) {
        if (value == null || value == '') {
            return value;
        }
        else {
            var neg = null;
            value = this.removeFormat(value);
            value = (neg = value < 0) ? value * -1 : value;

            if (this.useThousandSeparator) {
                value = value.toString();

                var ps = value.split(Ext.util.Format.decimalSeparator);

                if (ps[1] != undefined) {
                    if (ps[1].length > this.decimalPrecision)
                        ps[1] = ps[1].substring(0, this.decimalPrecision);
                }

                var whole = ps[0];

                var r = /(\d+)(\d{3})/;

                var ts = Ext.util.Format.thousandSeparator;

                while (r.test(whole))
                    whole = whole.replace(r, '$1' + ts + '$2');

                value = whole + (ps[1] != undefined ? Ext.util.Format.decimalSeparator + ps[1] : '');
            }

            return Ext.String.format('{0}{1}{2}', (neg ? '-' : ''), (Ext.isEmpty(this.currencySymbol) ? '' : this.currencySymbol + ' '), value);
        }
    },
    /**
    * overrides parseValue to remove the format applied by this class
    */
    parseValue: function (value) {
        //Replace the currency symbol and thousand separator
        if (value == 'N/A')
            return null;
        else {
            return this.removeFormat(value);
        }
        
    },
    //        /**
    //        * Remove only the format added by this class to let the superclass validate with it's rules.
    //        * @param {Object} value
    //        */
    onBlur: function () {
        if (this.getRawValue() == null || this.getRawValue() == '')
            this.setRawValue('N/A');
    },
    removeFormat: function (value) {
        if (Ext.isEmpty(value) || !this.hasFormat())
            return value;
        else {
            value = value.toString().replace(this.currencySymbol + ' ', '');

            value = this.useThousandSeparator ? value.replace(new RegExp('[' + Ext.util.Format.thousandSeparator + ']', 'g'), '') : value;

            return value;
        }
    },
    //        /**
    //        * Remove the format before validating the the value.
    //        * @param {Number} value
    //        */
    getErrors: function (value) {
        return Ext.form.field.Number.superclass.getErrors.call(this, this.removeFormat(value));
    },
    hasFormat: function () {
        return this.decimalSeparator != '.' || (this.useThousandSeparator == true && this.getRawValue() != null) || !Ext.isEmpty(this.currencySymbol) || this.alwaysDisplayDecimals;
    },
    //        /**
    //        * Display the numeric value with the fixed decimal precision and without the format using the setRawValue, don't need to do a setValue because we don't want a double
    //        * formatting and process of the value because beforeBlur perform a getRawValue and then a setValue.
    //        */
    onFocus: function () {
        this.setRawValue(this.getRawValue().replace('N/A', ''));

        this.callParent(arguments);
    }
});

//function getComboEditor(grid, colIndex) {
//    var gridCols = grid.columns;
//    var combo = gridCols[colIndex].getEditor();
//    return combo;
//}

//var comboRenderer = function (value, colIndex, grid) {
//    var combo = getComboEditor(grid, colIndex);
//    var record = combo.findRecord(combo.valueField, value);
//    return record ? record.get(combo.displayField) : combo.valueNotFoundText;
//};

//var comboRenderer2 = function (value, dataIndex, grid) {
//    try {
//        var combo = grid.down('[dataIndex=' + dataIndex + ']').getEditor();
//        var record = combo.findRecord(combo.valueField, value);
//        return record ? record.get(combo.displayField) : combo.valueNotFoundText;
//    }
//    catch (e) { }
//};


function reset_post2() {
    try {
        if (reset_post) {
            reset_post();
            return;
        }
        if (parent && parent.reset_post2)
            parent.reset_post2();
    }
    catch (e) {
    }
}
//<![CDATA[
document.onmousemove = function () {
    try {
        reset_post2 && reset_post2();
    }
    catch (e) {
    }
}
//]]>

function printPanel(ID) {
    var win = window.open('', 'Print Panel');
    win.document.open();
    win.document.write(
    "<head><link href='../Styles/Site.Master.css' rel='stylesheet' type='text/css' /><link href='../Themes/Accent/resources/css/accent.css' rel='stylesheet' type='text/css' />" + "</head><body>" + Ext.getCmp(ID).body.dom.innerHTML + "</body>");
    win.document.close();
    win.print();
    win.setTimeout("close()", 1000);
    //win.close(); 
}

var fnNumber = function (value) {
    if (value == null || value == undefined) return value;
    if (value.toString().indexOf(".") < 0)
        digit = '0,000';
    else
        digit = '0,000.00'
    return Ext.String.format(template, (value >= 0) ? "" : "red", Ext.util.Format.number(value, digit));
};

var _loadForm = function (form, record) {
    try {
        form.loadRecord(record);


        for (var propertyName in record.data) {
            obj = form.getForm().findField(propertyName);
            if (obj != undefined && obj != null) {
                if (obj.multiSelect != undefined && obj.multiSelect == true) {
                    value = record.data[propertyName].split(",")
                    obj.setValue(value);
                }
            }
        }
        return true;
    }
    catch (e) {
        alert(e.Message);
    }
};

var _reset = function (form, grid) {
    form.getForm()._record = null;
    form.getForm().reset();
};


var _storeJSON = function (store) {
    var data = [];
    store.each(function (r) {
        data.push(r.data);
    });
    return Ext.encode(data);
};


var _update = function (form, grid, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {

    if (!form.getForm().isValid())
        return false;

    if (form.getForm().getRecord() != null)
        form.getForm().updateRecord(form.getForm().getRecord());
    else
        grid.store.add(form.getForm().getFieldValues(false, "dataIndex"));

    if (AJAXRequest != undefined) {
        var rec;
        parms = [];
        var updatedRecords = grid.store.getUpdatedRecords();
        Ext.each(updatedRecords, function (record) {
            rec = record;
            parms.push(record.data);
        });
        var newRecords = grid.store.getNewRecords();
        Ext.each(newRecords, function (record) {
            rec = record;
            parms.push(record.data);
        });
        if (parms.length > 0) {
            form.el.mask('Saving Your Changes...');
            grid.el.mask('Saving Your Changes...');

            Ext.net.DirectMethod.request(
            AJAXRequest,
            {
                params: {
                    'action': 'update',
                    'records': Ext.encode(parms)
                },
                success: function (result) {
                    if (onSaveChangesSuccess != undefined)
                        onSaveChangesSuccess(result, rec);
                    try {
                        var recResult = Ext.JSON.decode(result);
                        for (var propertyName in recResult) {
                            if (recResult[propertyName].indexOf('/Date') == 0) {
                                recResult[propertyName] = new Date(parseInt(recResult[propertyName].replace('/Date(', '')));
                            }
                            rec.set(propertyName, recResult[propertyName]);
                        }
                    }
                    catch (e) {

                    }
                    grid.store.commitChanges();
                    Ext.Msg.alert("Notification", "Save Success");
                    form.getForm().reset();
                    form.el.unmask();
                    grid.el.unmask();
                },
                failure: function (msg, options) {
                    if (onSaveChangesFailure != undefined)
                        onSaveChangesFailure(msg, rec);
                    grid.store.rejectChanges();
                    Ext.Msg.alert("Error", "Save Failed");
                    form.el.unmask();
                    grid.el.unmask();
                    //Failure code goes here
                }
            });
        }
    }

    return true;

};

var _delete = function (form, grid, record, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {
Ext.Msg.confirm('Delete Confirmation', 'Are you sure?', function (id, value) {
if (id === 'yes') {

    grid.store.remove(record);

    if (AJAXRequest != undefined) {
        parms = [];
        var updatedRecords = grid.store.getRemovedRecords();
        Ext.each(updatedRecords, function (record) {
            parms.push(record.data);
        });

        if (parms.length > 0) {
            form.el.mask('Saving Your Changes...');
            grid.el.mask('Saving Your Changes...');

            Ext.net.DirectMethod.request(
            AJAXRequest,
            {
                params: {
                    'action': 'delete',
                    'records': Ext.encode(parms)
                },
                success: function (result) {
                    if (onSaveChangesSuccess != undefined)
                        onSaveChangesSuccess(result);
                    Ext.Msg.alert("Notification", "Delete Success");
                    grid.store.commitChanges();
                    form.el.unmask();
                    grid.el.unmask();
                },
                failure: function (msg, options) {
                    if (onSaveChangesFailure != undefined)
                        onSaveChangesFailure(msg);
                    grid.store.rejectChanges();
                    Ext.Msg.alert("Error", "Delete Failed");
                    form.el.unmask();
                    grid.el.unmask();
                    //Failure code goes here
                }
            });
        }
    }

}}, this);

};

var _MakerUpdate = function (form, grid, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {

    if (!form.getForm().isValid())
        return false;

    var rec = Ext.create(grid.store.model);
    form.getForm().updateRecord(rec);
    rec.set('__ACTION__', 'Update');

    if (AJAXRequest != undefined) {
        form.el.mask('Saving Your Changes...');
        Ext.net.DirectMethod.request(
            AJAXRequest,
            {
                params: {
                    'action': 'Update',
                    'records': "[" + Ext.encode(rec.data) + "]"
                },
                success: function (result) {
                    try {
                        if (onSaveChangesSuccess != undefined)
                            onSaveChangesSuccess(result, rec);

                        var recResult = Ext.JSON.decode(result);
                        for (var propertyName in recResult) {
                            if (recResult[propertyName] != null && recResult[propertyName].indexOf('/Date') == 0) {
                                recResult[propertyName] = new Date(parseInt(recResult[propertyName].replace('/Date(', '')));
                            }
                            rec.set(propertyName, recResult[propertyName]);
                        }
                        try {
                            var updateRecord = grid.store.findRecord('__TABLEKEY__', recResult["__TABLEKEY__"]);
                            if (updateRecord)
                                updateRecord.set(rec.data);
                            else
                                grid.store.addSorted(rec);
                        }
                        catch (e) { }
                        grid.store.commitChanges();

                        Ext.Msg.alert("Notification", "Save Success");
                        form.el.unmask();
                    }
                    catch (e) {
                        Ext.Msg.alert("Error", e.message);
                    }
                },
                failure: function (msg, options) {
                    if (onSaveChangesFailure != undefined)
                        onSaveChangesFailure(msg, rec);
                    Ext.Msg.alert("Error", "Save Failed");
                    form.el.unmask();
                }
            }
        );
    }
    return true;
};

var _MakerUpdateTemp = function (form, grid, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {

    if (!form.getForm().isValid())
        return false;

    var rec = Ext.create(grid.store.model);
    var Values = form.getForm().getValues();
    rec.set(Values);
    rec.set('__ACTION__', 'Update');

    if (AJAXRequest != undefined) {
        form.el.mask('Saving Your Changes...');
        Ext.net.DirectMethod.request(
            AJAXRequest,
            {
                params: {
                    'action': 'Update',
                    'records': "[" + Ext.encode(rec.data) + "]"
                },
                success: function (result) {
                    try {
                        if (onSaveChangesSuccess != undefined)
                            onSaveChangesSuccess(result, rec);

                        grid.store.load();

                        Ext.Msg.alert("Notification", "Save Success");
                        form.el.unmask();
                    }
                    catch (e) {
                        Ext.Msg.alert("Error", e.message);
                    }
                },
                failure: function (msg, options) {
                    if (onSaveChangesFailure != undefined)
                        onSaveChangesFailure(msg, rec);
                    Ext.Msg.alert("Error", "Save Failed");
                    form.el.unmask();
                }
            }
        );
    }
    return true;
};

var _MakerDelete = function (form, grid, record, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {
Ext.Msg.confirm('Delete Confirmation', 'Are you sure?', function (id, value) {
if (id === 'yes') {

    var rec = Ext.create(grid.store.model);
    rec.set(record.data);
    rec.set('__ACTION__', 'Delete');

    if (AJAXRequest != undefined) {
        Ext.net.DirectMethod.request(
        AJAXRequest,
        {
            params: {
                'action': 'Delete',
                'records': "[" + Ext.encode(rec.data) + "]"
            },
            success: function (result) {
                try {
                    if (onSaveChangesSuccess != undefined)
                        onSaveChangesSuccess(result, rec);

                    var recResult = Ext.JSON.decode(result);
                    for (var propertyName in recResult) {
                        if (recResult[propertyName] != null && recResult[propertyName].indexOf('/Date') == 0) {
                            recResult[propertyName] = new Date(parseInt(recResult[propertyName].replace('/Date(', '')));
                        }
                        rec.set(propertyName, recResult[propertyName]);
                    }

                    try {
                        var updateRecord = grid.store.findRecord('__TABLEKEY__', recResult["__TABLEKEY__"]);
                        if (updateRecord)
                            updateRecord.set(rec.data);
                        else
                            grid.store.addSorted(rec);
                    }
                    catch (e) { }

                    grid.store.commitChanges();

                    Ext.Msg.alert("Notification", "Delete Success");
                    form.el.unmask();
                }
                catch (e) {
                    Ext.Msg.alert("Error", e.message);
                }
            },
            failure: function (msg, options) {
                if (onSaveChangesFailure != undefined)
                    onSaveChangesFailure(msg, rec);
                Ext.Msg.alert("Error", "Delete Failed");
            }
        });
    }
}}, this);

};

var _DeleteConfirm = function (record) {

    Ext.Msg.confirm('Delete Confirmation', 'Are you sure?',
    function (id, value) {
        if (id === 'yes') {
            return true;
        }
    });
    return false;
}


var _DeleteLocal = function (form, grid, record) {

    Ext.Msg.confirm('Delete Confirmation', 'Are you sure?',
    function (id, value) {
        if (id === 'yes') {
            grid.store.remove(record);
        }
    });
}

var _UpdateLocal = function (form, grid) {

    if (!form.getForm().isValid())
        return false;

    if (form.getForm().getRecord() != null)
        form.getForm().updateRecord(form.getForm().getRecord());
    else
        grid.store.add(form.getForm().getFieldValues(false, "dataIndex"));
    return true;
}



var _MakerDeleteTemp = function (form, grid, record, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {

Ext.Msg.confirm('Delete Confirmation', 'Are you sure?', function (id, value) {
if (id === 'yes') {
    
    var rec = Ext.create(grid.store.model);
    rec.set(record.data);
    rec.set('__ACTION__', 'Delete');
    if (AJAXRequest != undefined) {
        Ext.net.DirectMethod.request(
        AJAXRequest,
        {
            params: {
                'action': 'Delete',
                'records': "[" + Ext.encode(rec.data) + "]"
            },
            success: function (result) {
                try {
                    if (onSaveChangesSuccess != undefined)
                        onSaveChangesSuccess(result, rec);

                    grid.store.load();

                    Ext.Msg.alert("Notification", "Delete Success");
                }
                catch (e) {
                    Ext.Msg.alert("Error", e.message);
                }
            },
            failure: function (msg, options) {
                if (onSaveChangesFailure != undefined)
                    onSaveChangesFailure(msg, rec);
                Ext.Msg.alert("Error", "Delete Failed");
            }
        });
    }

}}, this);

};


var _CheckerApprove = function (grid, grid2, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {
if (grid.getView().getSelectionModel().getSelection().length == 0) {
    Ext.Msg.alert("Notification", "There is no data to approve");
    return;
}
Ext.Msg.confirm('Approve Confirmation', 'Are you sure?', function (id, value) {
if (id === 'yes') {

    var selections = grid.getView().getSelectionModel().getSelection();
    parms = [];
    for (i = 0; i < selections.length; i++) {
        parms.push(selections[i].data.__TABLEKEY__);
    }

    if (AJAXRequest != undefined) {
        grid.el.mask('Saving Your Changes...');
        Ext.net.DirectMethod.request(
        AJAXRequest,
        {
            params: {
                'action': 'Approve',
                'records': Ext.encode(parms)
            },
            success: function (result) {
                try {
                    for (i = selections.length - 1; i >= 0; i--) {
                        _fMaker = true;
                        if (onSaveChangesSuccess != undefined)
                            _fMaker = onSaveChangesSuccess(result, selections[i]);
                        if (_fMaker) {
                            var updateRecord = grid2.store.findRecord('__TABLEKEY__', selections[i].data.__TABLEKEY__);
                            if (selections[i].data.__ACTION__ == "Delete")
                                grid2.store.remove(updateRecord);
                            if (selections[i].data.__ACTION__ == "Update") {
                                if (updateRecord) {
                                    updateRecord.set(selections[i].data);
                                }
                                else {
                                    var rec = Ext.create(grid2.store.model);
                                    rec.set(selections[i].data);
                                    grid2.store.addSorted(rec);
                                }
                            }
                        }
                    }
                    grid2.store.commitChanges();

                    for (i = selections.length - 1; i >= 0; i--) {
                        grid.store.remove(selections[i]);
                    }

                    grid.store.commitChanges();
                    Ext.Msg.alert("Notification", "Approve Success");
                    grid.el.unmask();
                }
                catch (e) {
                    Ext.Msg.alert("Error", e.message);
                }
            },
            failure: function (msg, options) {
                if (onSaveChangesFailure != undefined)
                    onSaveChangesFailure(msg, rec);
                Ext.Msg.alert("Error", "Approve Failed");
                grid.el.unmask();
            }
        });
    }

}}, this);

};


var _CheckerApproveTemp = function (grid, grid2, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {
    if (grid.getView().getSelectionModel().getSelection().length == 0) {
        Ext.Msg.alert("Notification", "There is no data to approve");
        return;
    }
    Ext.Msg.confirm('Approve Requirement', 'Are you sure?', function (id, value) {
        if (id === 'yes') {

            var selections = grid.getView().getSelectionModel().getSelection();
            parms = [];
            for (i = 0; i < selections.length; i++) {
                parms.push(selections[i].data.__TABLEKEY__);
            }
            if (AJAXRequest != undefined) {
                grid.el.mask('Saving Your Changes...');
                Ext.net.DirectMethod.request(
                AJAXRequest,
                {
                    params: {
                        'action': 'Approve',
                        'records': Ext.encode(parms)
                    },
                    success: function (result) {
                        try {

                            grid2.store.load();
                            grid.store.load();

                            Ext.Msg.alert("Notification", "Approve Success");
                            grid.el.unmask();
                        }
                        catch (e) {
                            Ext.Msg.alert("Error", e.message);
                        }
                    },
                    failure: function (msg, options) {
                        if (onSaveChangesFailure != undefined)
                            onSaveChangesFailure(msg, rec);
                        Ext.Msg.alert("Error", "Approve Failed");
                        grid.el.unmask();
                    }
                });
            }

        }
    }, this);

};

var _CheckerRejectTemp = function (grid, grid2, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {
    if (grid.getView().getSelectionModel().getSelection().length == 0) {
        Ext.Msg.alert("Notification", "There is no data to reject ");
        return;
    }
    Ext.Msg.confirm('Reject Confirmation', 'Are you sure?', function (id, value) {
        if (id === 'yes') {

            var selections = grid.getView().getSelectionModel().getSelection();
            parms = [];
            for (i = 0; i < selections.length; i++) {
                parms.push(selections[i].data.__TABLEKEY__);
            }

            if (AJAXRequest != undefined) {
                grid.el.mask('Saving Your Changes...');
                Ext.net.DirectMethod.request(
                AJAXRequest,
                {
                    params: {
                        'action': 'Reject',
                        'records': Ext.encode(parms)
                    },
                    success: function (result) {
                        try {
                            if (onSaveChangesSuccess != undefined)
                                onSaveChangesSuccess(result, rec);
                            grid.store.load();
                            grid.el.unmask();
                        }
                        catch (e) {
                            Ext.Msg.alert("Error", e.message);
                        }
                    },
                    failure: function (msg, options) {
                        if (onSaveChangesFailure != undefined)
                            onSaveChangesFailure(msg, rec);
                        Ext.Msg.alert("Error", "Reject Failed");
                        grid.el.unmask();
                    }
                });
            }

        } 
    }, this);

};


var _CheckerReject = function (grid, grid2, AJAXRequest, onSaveChangesSuccess, onSaveChangesFailure) {
if (grid.getView().getSelectionModel().getSelection().length == 0) {
    Ext.Msg.alert("Notification", "There is no data to reject ");
    return;
}
Ext.Msg.confirm('Reject Confirmation', 'Are you sure?', function (id, value) {
if (id === 'yes') {

    var selections = grid.getView().getSelectionModel().getSelection();
    parms = [];
    for (i = 0; i < selections.length; i++) {
        parms.push(selections[i].data.__TABLEKEY__);
    }

    if (AJAXRequest != undefined) {
        grid.el.mask('Saving Your Changes...');
        Ext.net.DirectMethod.request(
        AJAXRequest,
        {
            params: {
                'action': 'Reject',
                'records': Ext.encode(parms)
            },
            success: function (result) {
                try {
                    if (onSaveChangesSuccess != undefined)
                        onSaveChangesSuccess(result, rec);

                    for (i = selections.length - 1; i >= 0; i--) {
                        grid.store.remove(selections[i]);
                    }
                    grid.store.commitChanges();
                    Ext.Msg.alert("Notification", "Reject Success");
                    grid.el.unmask();
                }
                catch (e) {
                    Ext.Msg.alert("Error", e.message);
                }
            },
            failure: function (msg, options) {
                if (onSaveChangesFailure != undefined)
                    onSaveChangesFailure(msg, rec);
                Ext.Msg.alert("Error", "Reject Failed");
                grid.el.unmask();
            }
        });
    }

}}, this);

};

