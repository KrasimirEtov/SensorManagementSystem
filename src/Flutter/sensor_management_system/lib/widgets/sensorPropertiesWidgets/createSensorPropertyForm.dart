import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/services/webservice.dart';

class CreateSensorPropertyForm extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _CreateSensorPropertyFormState();
  }
}

class _CreateSensorPropertyFormState extends State<CreateSensorPropertyForm> {
  final _formKey = GlobalKey<FormState>();
  SensorProperty _model = SensorProperty();
  bool _isSwitch = false;

  void _createSensorProperty() {
    WebService().send(SensorProperty.initWithJsonBody(_model)).whenComplete(() {
    });
  }

  @override
  Widget build(BuildContext context) {
    return Form(
        key: _formKey,
        child: SingleChildScrollView(
            child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            TextFormField(
              decoration: InputDecoration(labelText: 'Measure Type'),
              validator: (value) {
                if (value.isEmpty) {
                  return 'Measure Type is required';
                }
                return null;
              },
              onChanged: (newValue) {
                _model.measureType = newValue;
              },
            ),
            TextFormField(
              decoration: InputDecoration(labelText: 'Measure Unit'),
              validator: (value) {
                if (value.isEmpty) {
                  return 'Measure Unit is required';
                }
                return null;
              },
              onChanged: (newValue) {
                _model.measureUnit = newValue;
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
                    _createSensorProperty();
                  }
                },
                child: Text('Create'),
              ),
            ),
          ],
        )));
  }
}
