import React, { useState } from "react";
import "./switchSelect.css";

export const SwitchSelect = ({
  Category,
  AddIDToArray,
  selectedCategoryIDs,
  disabled,
}) => {
  const [checked, setChecked] = useState(
    selectedCategoryIDs.includes(Category.id),
  );

  const handleOnChange = () => {
    setChecked((prevChecked) => {
      const newChecked = !prevChecked;
      const index = selectedCategoryIDs.indexOf(Category.id);
      if (!newChecked && index > -1) {
        const newSelectedCategoryIDs = [...selectedCategoryIDs];
        newSelectedCategoryIDs.splice(index, 1);
        AddIDToArray(newSelectedCategoryIDs);
      } else if (newChecked && index === -1 && selectedCategoryIDs.length < 3) {
        AddIDToArray([...selectedCategoryIDs, Category.id]);
      }
      return newChecked;
    });
  };

  const isDisabled =
    (!checked &&
      selectedCategoryIDs.indexOf(Category.id) === -1 &&
      selectedCategoryIDs.length === 3) ||
    disabled;

  return (
    <div className="card my-card">
      <div className="card-body my-card-body">
        <img
          src={Category.image}
          className="card-img-top my-img"
          alt="..."
        />
        <div className="form-check form-switch my-switch">
          <label className="form-check-label text">{Category.name}</label>
          <input
            className="form-check-input input"
            type="checkbox"
            id={`category-${Category.id}`}
            checked={checked}
            disabled={isDisabled}
            onChange={handleOnChange}
          />
        </div>
      </div>
    </div>
  );
};
