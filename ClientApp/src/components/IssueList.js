import React, { useEffect, useState } from 'react';

const IssueList = () => {
  const [data, setData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      console.debug("json");
      const response = await fetch('api/issue');
      const json = await response.json();
      console.debug(json);
      setData(json.issues);
    };

    fetchData();
  }, []);

  const formatDate = (date) => {
    const dateTime = new Date(date);
    return dateTime.toLocaleString('ja-JP', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit' });
  };

  return (
    <div>
      <h2>リスト2</h2>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>タイトル</th>
            <th>ステータス</th>
            <th>説明</th>
            <th>担当者</th>
            <th>期限</th>
            <th>優先度</th>
            <th>作成日</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.title}</td>
              <td>{item.status === 0 ? 'ToDo' : item.status === 1 ? 'WorkInProgress' : 'Done'}</td>
              <td>{item.description}</td>
              <td>{item.assignee}</td>
              <td>{formatDate(item.deadline)}</td>
              <td>{item.priority > 0 ? `+${item.priority}` : item.priority < 0 ? `${item.priority}` : '0'}</td>
              <td>{formatDate(item.createdAt)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default IssueList;
