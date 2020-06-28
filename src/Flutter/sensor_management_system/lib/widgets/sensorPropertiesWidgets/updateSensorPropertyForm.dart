import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/services/webservice.dart';

class UpdateSensorPropertyForm extends StatefulWidget {
  final String id;

  UpdateSensorPropertyForm({this.id});

  @override
  State<StatefulWidget> createState() {
    return _UpdateSensorPropertyFormState(this.id);
  }
}

class _UpdateSensorPropertyFormState extends State<UpdateSensorPropertyForm> {
  String id;
  final _formKey = GlobalKey<FormState>();
  SensorProperty _model = SensorProperty();
  List<SensorProperty> _sensorProperties = List<SensorProperty>();
  bool _isSwitch = false;
  String _tempMeasureType;
  String _tempMeasureUnit;
  Future _loadSensorProperty;

  _UpdateSensorPropertyFormState(String id) {
    this.id = id;
  }

  @override
  void initState() {
    super.initState();
    _loadSensorProperty = _populateSensorProperty();
    _populateSensorProperties();
  }

  Future _populateSensorProperty() {
    return WebService()
        .load(SensorProperty.initResourceByIdWithResponse(this.id))
        .then((sensorProperty) => {
              setState(() => {
                    _model = sensorProperty,
                    _isSwitch = sensorProperty.isSwitch.toLowerCase() == 'true'
                  })
            });
  }

  Future _populateSensorProperties() {
    return WebService().load(SensorProperty.all).then((sensorProperties) => {
          setState(() => {_sensorProperties = sensorProperties})
        });
  }

  void _updateSensorProperty() {
    WebService()
        .update(SensorProperty.initWithJsonBody(_model))
        .whenComplete(() {});
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder(
      future: _loadSensorProperty,
      builder: (context, snapshot) {
        if (snapshot.hasData) {
          return Form(
              key: _formKey,
              child: SingleChildScrollView(
                  child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  TextFormField(
                    initialValue: _model.measureType,
                    decoration: InputDecoration(labelText: 'Measure Type'),
                    validator: (value) {
                      if (value.isEmpty) {
                        return 'Measure Type is required';
                      }
                      if (_sensorProperties.any((element) =>
                          element.measureType == _tempMeasureType &&
                          element.measureUnit == _tempMeasureUnit &&
                          element.id != this.id)) {
                        return 'Measure Type with this Measure unit already exists!';
                      }
                      return null;
                    },
                    onChanged: (newValue) {
                      _model.measureType = newValue;
                      setState(() {
                        _tempMeasureType = newValue;
                      });
                    },
                  ),
                  TextFormField(
                    initialValue: _model.measureUnit,
                    decoration: InputDecoration(labelText: 'Measure Unit'),
                    validator: (value) {
                      if (value.isEmpty) {
                        return 'Measure Unit is required';
                      }
                      if (_sensorProperties.any((element) =>
                          element.measureType == _tempMeasureType &&
                          element.measureUnit == _tempMeasureUnit &&
                          element.id != this.id)) {
                        return 'Measure Type with this Measure unit already exists!';
                      }
                      return null;
                    },
                    onChanged: (newValue) {
                      _model.measureUnit = newValue;
                      setState(() {
                        _tempMeasureUnit = newValue;
                      });
                    },
                  ),
                  SwitchListTile(
                    title: Text('Is Switch'),
                    value: _isSwitch,
                    onChanged: (value) {
                      setState(() {
                        _isSwitch = value;
                      });
                      _model.isSwitch = value.toString();
                    },
                    secondary: const Icon(Icons.swap_horizontal_circle),
                  ),
                  Padding(
                    padding: const EdgeInsets.symmetric(vertical: 16.0),
                    child: RaisedButton(
                      onPressed: () {
                        // Validate returns true if the form is valid, or false
                        // otherwise.
                        if (_formKey.currentState.validate()) {
                          // If the form is valid, display a Snackbar.
                          Scaffold.of(context).showSnackBar(
                              SnackBar(content: Text('Processing Data')));
                          _updateSensorProperty();
                        }
                      },
                      child: Text('Update'),
                    ),
                  ),
                ],
              )));
        } else {
          return CircularProgressIndicator();
        }
      },
    );
  }
}
